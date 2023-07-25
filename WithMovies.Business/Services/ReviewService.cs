using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Business.Services
{
    public class ReviewService : IReviewService
    {
        private DataContext _dataContext;
        private ILogger<IReviewService> _logger;

        public ReviewService(DataContext dataContext, ILogger<IReviewService> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        public async Task Create(
            User user,
            Movie movie,
            double rating,
            string? message,
            DateTime postedTime
        )
        {
            Review reviewToAdd = new Review()
            {
                Author = user,
                Movie = movie,
                Rating = rating,
                Message = message,
                PostedTime = postedTime
            };
            await _dataContext.Reviews.AddAsync(reviewToAdd);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Review?> Read(int id) =>
            await _dataContext.Reviews.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Review>> ReadAll(int movieId) =>
            await _dataContext.Reviews.Where(m => m.Id == movieId).ToListAsync();

        public async Task Update(Review review)
        {
            _dataContext.Reviews.Update(review);

            await _dataContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (await Read(id) is Review review)
                _dataContext.Remove(review);

            await _dataContext.SaveChangesAsync();
        }
    }
}
