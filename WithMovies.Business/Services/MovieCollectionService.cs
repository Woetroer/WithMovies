using Microsoft.EntityFrameworkCore;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Business.Services
{
	public class MovieCollectionService : IMovieCollectionService
	{
        private DataContext _dataContext;

        public MovieCollectionService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<bool> MovieCollectionExistsAsync(string name)
        {
            return Task.FromResult(_dataContext.MovieCollections.Any(c => c.Name == name));
        }

        public Task<MovieCollection?> MovieCollectionGetByNameAsync(string name)
        {
            return _dataContext.MovieCollections.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<MovieCollection> MovieCollectionCreateAsync(string name, string? posterPath = null, string? backdropPath = null, Movie[]? movies = null)
        {
            var collection = new MovieCollection
            {
                Name = name,
                PosterPath = posterPath,
                BackdropPath = backdropPath,
                Movies = movies?.ToList() ?? new(),
            };

            await _dataContext.AddAsync(collection);

            return collection;
        }
    }
}

