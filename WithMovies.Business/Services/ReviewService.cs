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
        private ILogger<MovieService> _logger;

        public ReviewService(DataContext dataContext, ILogger<MovieService> logger)
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
            var reviewToAdd = new Review
            {
                Author = user,
                Movie = movie,
                Rating = rating,
                Message = message,
                PostedTime = postedTime
            };

            movie.VoteAverage = ((movie.VoteAverage * movie.VoteCount) + rating) / ++movie.VoteCount;

            _dataContext.Update(movie);
            await _dataContext.AddAsync(reviewToAdd);
            await _dataContext.SaveChangesAsync();
        }

        public Task<Review?> Read(int id) =>
            _dataContext.Reviews.FindAsync(id).AsTask();

        public Task<List<Review>> ReadAll(int movieId) =>
            _dataContext.Reviews.Where(m => m.Id == movieId).ToListAsync();

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
