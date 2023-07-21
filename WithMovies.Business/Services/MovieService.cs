using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WithMovies.Domain.Enums;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Business.Services
{
    class MovieCollectionImport
    {
        public required string Name { get; set; }
        public required string? PosterPath { get; set; }
        public required string? BackdropPath { get; set; }
    }

    class MovieImport
    {
        public required int Id { get; set; }
        public required string ImdbId { get; set; }
        public required bool Adult { get; set; }
        public MovieCollectionImport? BelongsToCollection { get; set; }
        public required int Budget { get; set; }
        public required List<Genre> Genres { get; set; }
        public string? Homepage { get; set; }
        public string? OriginalLanguage { get; set; }
        public required string OriginalTitle { get; set; }
        public required string Overview { get; set; }
        public required double Popularity { get; set; }
        public string? PosterPath { get; set; }
        public required List<ProductionCompany> ProductionCompanies { get; set; }
        public required List<string?> ProductionCountries { get; set; }
        public int[]? ReleaseDate { get; set; }
        public required ulong Revenue { get; set; }
        public int[]? Runtime { get; set; }
        public required List<string?> SpokenLanguages { get; set; }
        public MovieStatus? MovieStatus { get; set; }
        public required string Tagline { get; set; }
        public required string Title { get; set; }
        public string? Video { get; set; }
        public required double VoteAverage { get; set; }
        public required int VoteCount { get; set; }
    }

    public class MovieService : IMovieService
    {
        private IMovieCollectionService _movieCollectionService;
        private IProductionCompanyService _productionCompanyService;
        private DataContext _dataContext;
        private ILogger<MovieService> _logger;

        public MovieService(
            DataContext dataContext,
            IMovieCollectionService movieCollectionService,
            IProductionCompanyService productionCompanyService,
            ILogger<MovieService> logger
        )
        {
            _dataContext = dataContext;
            _movieCollectionService = movieCollectionService;
            _productionCompanyService = productionCompanyService;
            _logger = logger;
        }

        public async Task ImportJsonAsync(Stream json)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new GenericEnumJsonConverter<Genre>());
            options.Converters.Add(new GenericEnumJsonConverter<MovieStatus>());

            var movies = new List<Movie>();
            var movieImports = (
                await JsonSerializer.DeserializeAsync<List<MovieImport>>(json, options)
            )!;

            // for progress bar
            double progress = 0.0;
            double step = 1.0 / (movieImports.Count - 1);
            int iteration = 0;

            foreach (MovieImport import in movieImports)
            {
                progress += step;
                iteration++;

                //if (import is null)
                //    throw new ArgumentException("Incorrect import");

                // There are a few duplicate rows in the dataset
                if (movies.Any(m => m.Id == import.Id))
                    continue;

                MovieCollection? collection = null;

                if (import.BelongsToCollection is MovieCollectionImport collectionImport)
                {
                    if (
                        !await _movieCollectionService.MovieCollectionExistsAsync(
                            collectionImport.Name
                        )
                    )
                    {
                        collection = await _movieCollectionService.MovieCollectionCreateAsync(
                            collectionImport.Name,
                            collectionImport.PosterPath,
                            collectionImport.BackdropPath
                        );
                    }
                    else
                    {
                        collection = await _movieCollectionService.MovieCollectionGetByNameAsync(
                            collectionImport.Name
                        );
                    }
                }

                if (iteration % 500 == 0)
                {
                    string progressBar = $"|{new string('=', (int)(progress * 10.0)) + ">",-11}|";
                    _logger.LogInformation($"{progressBar} Adding movies");
                }

                movies.Add(
                    new Movie
                    {
                        Id = import.Id,
                        ImdbId = import.ImdbId,
                        Adult = import.Adult,
                        BelongsToCollection = collection,
                        Budget = import.Budget,
                        Genres = import.Genres,
                        HomePage = import.Homepage,
                        OriginalLanguage = import.OriginalLanguage,
                        OriginalTitle = import.OriginalTitle,
                        Overview = import.Overview,
                        Popularity = import.Popularity,
                        PosterPath = import.PosterPath,
                        ProductionCompanies = import.ProductionCompanies
                            .Select(c => new ProductionCompany { Name = c.Name })
                            .ToList(),
                        ProductionCountries = import.ProductionCountries,
                        ReleaseDate =
                            import.ReleaseDate != null
                                ? new DateTime(import.ReleaseDate[0], 1, 1).AddDays(
                                    import.ReleaseDate[1]
                                )
                                : null,
                        Revenue = import.Revenue,
                        Runtime =
                            import.Runtime != null
                                ? new TimeSpan(0, 0, import.Runtime[0], import.Runtime[1])
                                : null,
                        SpokenLanguages = import.SpokenLanguages,
                        Status = import.MovieStatus ?? MovieStatus.None,
                        Tagline = import.Tagline,
                        Title = import.Title,
                        VoteAverage = import.VoteAverage,
                        VoteCount = import.VoteCount,
                    }
                );
            }

            await _dataContext.AddRangeAsync(movies);
        }

        public Task<Movie?> GetById(int movieId) => _dataContext.Movies.FindAsync(movieId).AsTask();

        public Task<List<Movie>> GetPopularMovies() =>
            _dataContext.Movies.OrderByDescending(movie => movie.VoteCount).Take(50).ToListAsync();
    }
}
