using WithMovies.Domain.Models;

namespace WithMovies.Domain.Interfaces
{
    public interface IMovieService
    {
        Task ImportJsonAsync(Stream json);
        Task<IQueryable<Movie>> GetTrending(int start, int limit);
        Task<IQueryable<Movie>> GetTrending(User user, int start, int limit);
        Task<IQueryable<Movie>> GetFriendMovies(User user);
        Task<IQueryable<Movie>> GetWatchList(User user);
        Task<Movie?> GetByIdAsync(int id);
    }
}
