using System.Text.Json;
using Microsoft.Extensions.Logging;
using WithMovies.Domain;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Business.Services
{
    public class KeywordService : IKeywordService
    {
        private DataContext _dataContext;
        private ILogger<KeywordService> _logger;

        public KeywordService(DataContext dataContext, ILogger<KeywordService> logger)
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
                        Movies = import.Value.Select(id => _dataContext.Movies.Find(id)!).ToList(),
                    }
                );
            }

            await _dataContext.Keywords.AddRangeAsync(keywords);
        }

        public Task<List<Keyword>> FindKeywordGroups(string text)
        {
            throw new NotImplementedException();
        }

        public Task<List<KeywordSuggestion>> FindKeywordSuggestions(string text)
        {
            throw new NotImplementedException();
        }
    }
}
