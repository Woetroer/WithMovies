namespace WithMovies.Domain.Interfaces;

public interface ICreditsService
{
    public Task ImportJsonAsync(Stream json);
}
