using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyModel;
using WithMovies.Business;
using WithMovies.Business.Services;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;
using WithMovies.WebApi.Models;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : MyControllerBase
    {
        private readonly IRecommendationService _recommendationService;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly DataContext _dataContext;

        public RecommendationsController(
            UserManager<User> userManager,
            IUserService userService,
            DataContext dataContext,
            IRecommendationService recommendationService
        )
        {
            _userManager = userManager;
            _userService = userService;
            _dataContext = dataContext;
            _recommendationService = recommendationService;
        }

        public record UserPreferencesArray(bool[] Preferences, bool Adult);

        [HttpPost("preferences")]
        public async Task<IActionResult> SetUserPreferences(UserPreferencesArray prefs)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
                return Unauthorized();

            await _userService.SetPreferencesAsync(prefs.Preferences, user);
            await _dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("run")]
        public async Task<IActionResult> Run()
        {
            await _recommendationService.RunRecommendationEngine();

            return Ok("Run complete");
        }
    }
}
