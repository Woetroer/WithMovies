using WithMovies.Domain.Models;

namespace WithMovies.Domain.Interfaces
{
	public interface IMovieCollectionService
	{
		Task<bool> MovieCollectionExistsAsync(string name);
		Task<MovieCollection> MovieCollectionCreateAsync(string name, string? posterPath = null, string? backdropPath = null, Movie[]? movies = null);
		Task<MovieCollection?> MovieCollectionGetByNameAsync(string name);
	}
}

