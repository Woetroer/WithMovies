using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;
using WithMovies.WebApi.Dtos;
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

        public ReviewController(IReviewService reviewService, IMovieService movieService, UserManager<User> userManager)
        {
            _reviewService = reviewService;
            _movieService = movieService;
            _userManager = userManager;
        }

        public record ReviewToAdd(int MovieId, double Rating, string? Message);

        [HttpPost("create"), Authorize]
        public async Task<IActionResult> CreateReview(ReviewToAdd reviewToAdd)
        {
            var movie = await _movieService.GetById(reviewToAdd.MovieId);

            if (movie == null)
                return NotFound("This movie doesn't exist");

            var user = _userManager.Users.First(x => x.UserName == User.Identity!.Name!);

            if (user.CanReview!)
                return Unauthorized("You've been blocked from making more reviews.");

            Movie movie = await _movieService.GetById(reviewToAdd.MovieId);

            if (user.CanReview)
                await _reviewService.Create(user, movie, reviewToAdd.Rating, reviewToAdd.Message, DateTime.Now);

            return Ok("Your review is added!");
        }

        [HttpGet("movie/{id}")]
        public async Task<IActionResult> ReadReviews(int id)
        {
            List<ReviewDto> dto = new List<ReviewDto>();
            List<Review> reviews = await _reviewService.ReadAll(id);
            foreach (Review review in reviews)
                dto.Add(review.ToDto());

            return Ok(dto);
        }

        public record UpdateArgs(int ReviewId, int MovieId, int Rating, string? Message);

        [HttpPost("edit"), Authorize]
        public async Task<IActionResult> UpdateReview(UpdateArgs reviewToUpdate)
        {
            var review = await _reviewService.Read(reviewToUpdate.ReviewId);

            if (review == null)
                return NotFound();

            if (review.Author.UserName != User.Identity!.Name!)
                return Unauthorized();

            review.Message = reviewToUpdate.Message;
            review.Rating = reviewToUpdate.Rating;

            await _reviewService.Update(review);

            return Ok();
        }

        [HttpDelete, Authorize(Roles = "Admin")]
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
