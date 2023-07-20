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
        Task Create(User userId, Movie movie, int rating, string? message, DateTime postedTime);
        Task<Review> Read(int id);
        Task<List<Review>> ReadAll(int id);
        Task Update(Review review);
        Task Delete(int id);
    }
}
