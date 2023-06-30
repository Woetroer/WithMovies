using System.Text.Json;
using WithMovies.Domain.Enums;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Business.Services
{
    class MovieImport
    {
        public required int Id { get; set; }
        public required string ImdbId { get; set; }
        public required bool Adult { get; set; }
        public required MovieCollection? BelongsToCollection { get; set; }
        public required int Budget { get; set; }
        public required List<Genre> Genres { get; set; }
        public required string? Homepage { get; set; }
        public required string? OriginalLanguage { get; set; }
        public required string OriginalTitle { get; set; }
        public required string Overview { get; set; }
        public required double Popularity { get; set; }
        public required string? PosterPath { get; set; }
        public required List<ProductionCompany> ProductionCompanies { get; set; }
        public required List<string?> ProductionCountries { get; set; }
        public required int[]? ReleaseDate { get; set; }
        public required ulong Revenue { get; set; }
        public required int[]? Runtime { get; set; }
        public required List<string?> SpokenLanguages { get; set; }
        public required MovieStatus? MovieStatus { get; set; }
        public required string Tagline { get; set; }
        public required string Title { get; set; }
        public required string? Video { get; set; }
        public required double VoteAverage { get; set; }
        public required int VoteCount { get; set; }
    }

    public class MovieService : IMovieService
    {
        private DataContext _dataContext;

        public MovieService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task ImportJsonAsync(Stream json)
        {
            await foreach (MovieImport? import in JsonSerializer.DeserializeAsyncEnumerable<MovieImport>(json))
            {
                if (import is null)
                    throw new ArgumentException("Incorrect import");

                var movie = new Movie
                {
                    Id = import.Id,
                    ImdbId = import.ImdbId,
                    Adult = import.Adult,
                    BelongsToCollection = import.BelongsToCollection,
                    Budget = import.Budget,
                    Genres = import.Genres,
                    HomePage = import.Homepage,
                    OriginalLanguage = import.OriginalLanguage,
                    OriginalTitle = import.OriginalTitle,
                    Overview = import.Overview,
                    Popularity = import.Popularity,
                    PosterPath = import.PosterPath,
                    ProductionCompanies = import.ProductionCompanies,
                    ProductionCountries = import.ProductionCountries,
                    ReleaseDate = import.ReleaseDate != null ? new DateTime(import.ReleaseDate[0], 0, import.ReleaseDate[1]) : null,
                    Revenue = import.Revenue,
                    Runtime = import.Runtime != null ? new TimeSpan(0, 0, import.Runtime[0], import.Runtime[1]) : null,
                    SpokenLanguages = import.SpokenLanguages,
                    Status = import.MovieStatus ?? MovieStatus.None,
                    Tagline = import.Tagline,
                    Title = import.Title,
                    VoteAverage = import.VoteAverage,
                    VoteCount = import.VoteCount,
                };
            }
        }

        public Task<Movie?> MovieGetById(int movieId) => _dataContext.Movies.FindAsync(movieId).AsTask();
    }
}

