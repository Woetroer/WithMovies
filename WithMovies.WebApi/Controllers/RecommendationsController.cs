using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;
using WithMovies.WebApi.Models;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : MyControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        public RecommendationsController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [Route("recommendation/preferences")]
        [HttpGet]
        public Task<IActionResult> GetUserPreferences(List<GenrePreference> preferences)
        {
            preferences.Remove(preferences.Where(x => x.Genre == "Adult").First());
            throw new NotImplementedException();

        }
    }
}
