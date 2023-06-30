using System.Text.Json;
using Microsoft.Extensions.Logging;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Business.Services
{
    class KeywordImport
    {
        public required List<(string keyword, int[] movies)> Keywords;
        public required int[] LenIndices;
    }

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
            var keywords = new List<Keyword>();
            var keywordImports = (await JsonSerializer.DeserializeAsync<KeywordImport>(json))!;

            // for progress bar
            double progress = 0.0;
            double step = 1.0 / (keywordImports.Keywords.Count() - 1);

            foreach (var import in keywordImports.Keywords)
            {
                progress += step;

                string progressBar = $"|{new string('=', (int)(progress * 10.0)) + ">",-11}|";
                _logger.LogInformation($"{progressBar} Adding {import.keyword}");

                keywords.Add(new Keyword
                {
                    Name = import.keyword,
                    Movies = import.movies.Select(id => _dataContext.Movies.Find(id)!).ToList(),
                });
            }

            await _dataContext.AddRangeAsync(keywords);
        }
    }
}

