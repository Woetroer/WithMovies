using WithMovies.Domain.Models;

namespace WithMovies.Domain.Interfaces
{
	public interface IMovieService
	{
		Task ImportJsonAsync(Stream json);
		Task<Movie> Get(int id);
	}
}

