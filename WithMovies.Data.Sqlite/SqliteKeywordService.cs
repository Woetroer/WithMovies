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
        private ILogger<SqliteKeywordService> _logger;

        public SqliteKeywordService(DataContext dataContext, ILogger<SqliteKeywordService> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
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
                    string progressBar = $"|{new string('=', (int)(progress * 10.0)) + ">", -11}|";
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

        public Task<IQueryable<Keyword>> FindKeywords(string[] names)
        {
            if (names.Length == 0)
                return Task.FromResult(Enumerable.Empty<Keyword>().AsQueryable());

            return Task.FromResult(_dataContext.Keywords.Where(k => names.Contains(k.Name)));
        }
    }
}
