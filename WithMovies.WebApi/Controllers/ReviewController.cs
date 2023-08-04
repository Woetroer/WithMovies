using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WithMovies.Business;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;
using WithMovies.WebApi.Extensions;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IMovieService _movieService;
        private readonly UserManager<User> _userManager;
        private readonly DataContext _dataContext;
        private readonly IRecommendationService _recommendationService;

        public ReviewController(
            IReviewService reviewService,
            IMovieService movieService,
            UserManager<User> userManager,
            DataContext dataContext,
            IRecommendationService recommendationService
        )
        {
            _reviewService = reviewService;
            _movieService = movieService;
            _userManager = userManager;
            _dataContext = dataContext;
            _recommendationService = recommendationService;
        }

        public record ReviewToAdd(int MovieId, double Rating, string? Message);

        [HttpPost("create"), Authorize]
        public async Task<IActionResult> CreateReview(ReviewToAdd reviewToAdd)
        {
            var movie = await _movieService.GetByIdAsync(reviewToAdd.MovieId);

            if (movie == null)
                return NotFound("This movie doesn't exist");

            var user = _userManager.Users.First(x => x.UserName == User.Identity!.Name!);

            if (!user.CanReview)
                return Unauthorized("You've been blocked from making more reviews.");

            await _reviewService.Create(
                user,
                movie,
                reviewToAdd.Rating,
                reviewToAdd.Message,
                DateTime.Now
            );

            await _recommendationService.FlagReviewedMovieAsync(user, movie, reviewToAdd.Rating);
            await _dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("movie/{id}")]
        public async Task<IActionResult> ReadReviews(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);

            if (movie == null)
                return NotFound();

            return Ok(
                movie.Reviews
                    .AsQueryable()
                    .OrderByDescending(r => r.PostedTime)
                    .Select(ReviewExtensions.ToDto)
            );
        }

        public record UpdateArgs(int ReviewId, int MovieId, int Rating, string? Message);

        [HttpPost("edit"), Authorize]
        public async Task<IActionResult> UpdateReview(UpdateArgs reviewToUpdate)
        {
            var review = await _reviewService.Read(reviewToUpdate.ReviewId);

            if (review == null)
                return NotFound();

            if (review.Author.UserName != User.Identity!.Name! || !review.Author.CanReview)
                return Unauthorized();

            review.Message = reviewToUpdate.Message;
            review.Rating = reviewToUpdate.Rating;

            await _reviewService.Update(review);
            await _recommendationService.FlagReviewedMovieAsync(
                review.Author,
                review.Movie,
                reviewToUpdate.Rating
            );
            await _dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("admin-delete"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDeleteReview(int id)
        {
            await _reviewService.Delete(id);
            return Ok();
        }

        [HttpDelete, Authorize]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _reviewService.Read(id);

            if (review == null)
                return NotFound();

            if (review.Author.UserName != User.Identity!.Name!)
                return Unauthorized();

            await _reviewService.Update(review);
            return Ok();
        }
    }
}
