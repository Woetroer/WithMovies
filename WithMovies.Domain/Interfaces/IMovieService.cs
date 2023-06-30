namespace WithMovies.Domain.Interfaces
{
	public interface IMovieService
	{
		Task ImportJsonAsync(Stream json);
	}
}

