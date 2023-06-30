using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WithMovies.Domain.Interfaces;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public record ReviewToAdd(int Userid, int MovieId, int Rating, string? Message, DateTime PosedTime);
        [HttpPost]
        public async Task<IActionResult>CreateReview(ReviewToAdd reviewToAdd)
        {

        }
    }
}
