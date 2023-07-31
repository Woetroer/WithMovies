using WithMovies.Domain.Models;
using WithMovies.WebApi.Dtos;
using WithMovies.Domain;

namespace WithMovies.WebApi.Extensions
{
    public static class MovieExtensions
    {
        public static MovieDto ToDto(this Movie movie)
        {
            MovieDto dto = new();
            dto.ImdbId = movie.ImdbId;
            if (movie.BelongsToCollection != null)
            {
                dto.BelongsToCollection = new MovieCollectionDto
                {
                    Id = movie.BelongsToCollection.Id,
                    Name = movie.BelongsToCollection.Name,
                    PosterPath = movie.BelongsToCollection.PosterPath,
                    BackdropPath = movie.BelongsToCollection.BackdropPath,
                    ItemCount = movie.BelongsToCollection.Movies.Count
                };
            }
            dto.Title = movie.Title;
            dto.Tagline = movie.Tagline;
            dto.OriginalLanguage = movie.OriginalLanguage;
            dto.OriginalTitle = movie.OriginalTitle;
            dto.Adult = movie.Adult;
            dto.Overview = movie.Overview;
            dto.Budget = movie.Budget;
            dto.Genres = movie.Genres.Cast<int>().ToList();
            dto.HomePage = movie.HomePage;
            dto.PosterPath = movie.PosterPath;
            dto.ProductionCompanies = movie.ProductionCompanies
                .Select(p => new NamedId { Id = p.Id, Name = p.Name })
                .ToList();
            dto.ProductionCountries = movie.ProductionCountries;
            dto.ReleaseDate = movie.ReleaseDate;
            dto.Revenue = movie.Revenue;
            dto.Runtime = movie.Runtime;
            dto.SpokenLanguages = movie.SpokenLanguages;
            dto.Status = movie.Status;
            dto.VoteAverage = movie.VoteAverage;
            dto.VoteCount = movie.VoteCount;
            dto.Popularity = movie.Popularity;
            dto.Keywords = movie.Keywords.Select(k => k.Name).ToList();

            return dto;
        }

        public static PreviewDto ToPreviewDto(Movie movie) =>
            new()
            {
                Id = movie.Id,
                Title = movie.Title,
                PosterPath = movie.PosterPath,
                Tagline = movie.Tagline,
            };
    }
}
