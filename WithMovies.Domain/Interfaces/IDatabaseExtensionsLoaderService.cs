namespace WithMovies.Domain.Interfaces;

public interface IDatabaseExtensionsLoaderService
{
    public Task EnsureLoaded(string extension);
}
