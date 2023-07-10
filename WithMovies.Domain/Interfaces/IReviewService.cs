using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WithMovies.Domain.Models;

namespace WithMovies.Domain.Interfaces
{
    public interface IReviewService
    {
        Task Create(int userId, int movieId, int rating, string? message, DateTime postedTime);
        Task<Review> Read(int id);
        Task Update(Review review);
        Task Delete(int id);
    }
}
