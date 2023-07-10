using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WithMovies.Domain.Models;
using WithMovies.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace WithMovies.Business.Services
{
    public class ReviewService : IReviewService
    {
        private DataContext _dataContext;
        private ILogger<MovieService> _logger;

        public async Task Create(int userId, int movieId, int rating, string? message, DateTime postedTime)
        {
            Review reviewToAdd = new Review()
            {
                UserId = userId,
                MovieId = movieId,
                Rating = rating,
                Message = message,
                PostedTime = postedTime
            };
            await _dataContext.Reviews.AddAsync(reviewToAdd);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Review> Read(int id)
        {
            return await _dataContext.Reviews.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task Delete(int id)
        {
            Review reviewToDelete = Read(id).Result;
            _dataContext.Reviews.Remove(reviewToDelete);
            await _dataContext.SaveChangesAsync();
        }
    }
}
