using WithMovies.Domain.Models;

namespace WithMovies.Domain.Interfaces
{
    public interface IReviewService
    {
        Task Create(User user, Movie movie, double rating, string? message, DateTime postedTime);
        Task<Review?> Read(int id);
        Task Update(Review review);
        Task Delete(int id);
    }
}
