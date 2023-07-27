using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyModel;
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
        public async Task<IActionResult> GetUserPreferences(List<GenrePreference> preferences)
        {
            preferences.Remove(preferences.Where(x => x.Genre == "Adult").First());
            await _userService.AddPreferencesAsync(preferences, _userManager.Users.First(x => x.UserName == User.Identity!.Name!));
            return Ok();
        }
    }
}
