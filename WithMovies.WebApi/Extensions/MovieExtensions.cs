using Microsoft.JSInterop.Infrastructure;
using System.Runtime.CompilerServices;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;
using WithMovies.WebApi.Dtos;


namespace WithMovies.WebApi.Extensions
{
    public static class MovieExtensions
    {
        public static MovieDto ToDto(this Movie movie)
        {
            MovieDto dto = new MovieDto();
            dto.ImdbId = movie.ImdbId;
            dto.BelongsToCollection = movie.BelongsToCollection;
            dto.Title = movie.Title;
            dto.Tagline = movie.Tagline;
            dto.OriginalLanguage = movie.OriginalLanguage;
            dto.OriginalTitle = movie.OriginalTitle;
            dto.Adult = movie.Adult;
            dto.Overview = movie.Overview;
            dto.Budget = movie.Budget;
            //dto.Genres = movie.Genres;
            dto.HomePage = movie.HomePage;
            dto.PosterPath = movie.PosterPath;
            //dto.ProductionCompanies = movie.ProductionCompanies;
            dto.ProductionCountries = movie.ProductionCountries;
            dto.ReleaseDate = movie.ReleaseDate;
            dto.Revenue = movie.Revenue;
            dto.Runtime = movie.Runtime;
            dto.SpokenLanguages = movie.SpokenLanguages;
            dto.Status = movie.Status;
            dto.VoteAverage = movie.VoteAverage;
            dto.VoteCount = movie.VoteCount;
            dto.Popularity = movie.Popularity;

            return dto;
        }
    }
}
