using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly UserManager<User> _userManager;
        public ReviewController(IReviewService reviewService, UserManager<User> userManager)
        {
            _reviewService = reviewService;
            _userManager = userManager;
        }

        public record ReviewToAdd(int MovieId, int Rating, string? Message);
        [HttpPost, Authorize]
        public async Task<IActionResult>CreateReview(ReviewToAdd reviewToAdd)
        {
            var user = _userManager.Users.First(x => x.UserName == User.Identity!.Name!);

            await _reviewService.Create(user.Id, reviewToAdd.MovieId, reviewToAdd.Rating, reviewToAdd.Message, DateTime.Now);

            return Ok("Your review is added!");
        }

        [HttpDelete, Authorize(Roles = "Admin")]
        public async Task<IActionResult>DeleteReview(int id)
        {
            await _reviewService.Delete(id);
            return Ok();
        }
    }
}
