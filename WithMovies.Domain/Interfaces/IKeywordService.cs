namespace WithMovies.Domain.Interfaces
{
	public interface IKeywordService
	{
		Task ImportJsonAsync(Stream json);
	}
}

