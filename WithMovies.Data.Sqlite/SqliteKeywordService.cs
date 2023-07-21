using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WithMovies.Business;
using WithMovies.Domain;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Data.Sqlite
{
    public class SqliteKeywordService : IKeywordService
    {
        private DataContext _dataContext;
        private IDatabaseExtensionsLoaderService _databaseExtensionsLoader;
        private ILogger<SqliteKeywordService> _logger;

        private static string BasePath = Path.GetDirectoryName(
            Assembly.GetExecutingAssembly().Location
        )!;
        private static string SuggestKeywordsScript = File.ReadAllText(
            Path.Combine(BasePath, "sql/suggest-keywords.sql")
        );

        public SqliteKeywordService(
            DataContext dataContext,
            ILogger<SqliteKeywordService> logger,
            IDatabaseExtensionsLoaderService databaseExtensionsLoader
        )
        {
            _dataContext = dataContext;
            _logger = logger;
            _databaseExtensionsLoader = databaseExtensionsLoader;
        }

        public async Task ImportJsonAsync(Stream json)
        {
            var keywordImports = (
                await JsonSerializer.DeserializeAsync<Dictionary<string, int[]>>(json)
            )!;
            var keywords = new List<Keyword>();

            // for progress bar
            double progress = 0.0;
            double step = 1.0 / (keywordImports.Count - 1);
            int iteration = 0;

            foreach (var import in keywordImports)
            {
                progress += step;
                iteration++;

                if (iteration % 500 == 0)
                {
                    string progressBar = $"|{new string('=', (int)(progress * 10.0)) + ">",-11}|";
                    _logger.LogInformation($"{progressBar} Adding keywords");
                }

                keywords.Add(
                    new Keyword
                    {
                        Name = import.Key,
                        Movies = import.Value
                            .Select(id => _dataContext.Set<Movie>().Find(id)!)
                            .ToList(),
                    }
                );
            }

            await _dataContext.AddRangeAsync(keywords);
        }

        public async Task<List<string>> FindKeywords(string text)
        {
            await _databaseExtensionsLoader.EnsureLoaded("fuzzy");
            await _databaseExtensionsLoader.EnsureLoaded("text");
            await _dataContext.Database.BeginTransactionAsync();

            var script = BuildFindKeywordsScript(text.Count(c => c == ' ') + 1);

            _logger.LogDebug("Running script:\n" + script);

            return await _dataContext.Database
                .SqlQueryRaw<string>(script, new SqliteParameter("text", text))
                .ToListAsync();
        }

        public async Task<List<KeywordSuggestion>> FindKeywordSuggestions(string text)
        {
            await _databaseExtensionsLoader.EnsureLoaded("fuzzy");
            await _databaseExtensionsLoader.EnsureLoaded("text");
            await _dataContext.Database.BeginTransactionAsync();

            var script = BuildSuggestKeywordsScript(text.Count(c => c == ' ') + 1, 0.85f);

            _logger.LogInformation("Running script:\n" + script);

            var keywords = await _dataContext
                .Set<KeywordRecord>()
                .FromSqlRaw(script, new SqliteParameter("text", text))
                .ToListAsync();

            return keywords
                .Select(k => new KeywordSuggestion(k.Name!, text, (float)k.Weight!))
                .ToList();
        }

        // Generates an SQL script that searches all possible keyword combinations
        private static string BuildFindKeywordsScript(int words)
        {
            StringBuilder builder = new();

            builder.Append("SELECT Name ");
            builder.Append("FROM Keywords WHERE translit(Name) = translit(:text) ");

            builder.AppendJoin(
                ' ',
                Enumerable
                    .Range(1, words)
                    .Select(i =>
                    {
                        StringBuilder subBuilder = new();

                        for (int j = 1; j < Math.Min(9, words - i + 1); j++)
                        {
                            subBuilder.Append("OR CONCAT(' ', text_join(' ', ");
                            subBuilder.AppendJoin(
                                ", ",
                                Enumerable
                                    .Range(1, Math.Min(j, words - i))
                                    .Select(k => $"text_split(translit(:text), ' ', {i + k})")
                            );
                            subBuilder.Append("), ' ') LIKE CONCAT('% ', translit(Name), ' %') ");
                        }

                        return subBuilder.ToString();
                    })
            );

            return builder.ToString();
        }

        private static string BuildSuggestKeywordsScript(int words, float weightThreshold)
        {
            StringBuilder builder = new();

            builder.Append("SELECT Name, max(");

            for (int i = 0; i < Math.Min(8, words); i++)
            {
                builder.Append("jaro_winkler(translit(Name), text_join(' '");

                for (int j = i; j >= 0; j--)
                {
                    builder.Append($", text_split(translit(:text), ' ', {words - j})");
                }

                builder.Append(")), ");
            }

            builder.Append($"-1) AS Weight FROM Keywords ");
            builder.Append(
                $"WHERE Weight > {weightThreshold.ToString(CultureInfo.InvariantCulture)} "
            );
            builder.Append("ORDER BY Weight DESC LIMIT 25;");

            return builder.ToString();
        }
    }
}
