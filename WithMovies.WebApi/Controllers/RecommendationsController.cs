namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : MyControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly DataContext _dataContext;

        public RecommendationsController(UserManager<User> userManager, IUserService userService, DataContext dataContext)
        {
            _userManager = userManager;
            _userService = userService;
            _dataContext = dataContext;
        }

        public record UserPreferencesArray(bool[] Preferences, bool Adult);

        [HttpPost("preferences")]
        public async Task<IActionResult> SetUserPreferences(UserPreferencesArray prefs)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
                return Unauthorized();

            await _userService.SetPreferencesAsync(
                prefs.Preferences,
                user
            );
            await _dataContext.SaveChangesAsync();

            return Ok();
        }
    }
}
