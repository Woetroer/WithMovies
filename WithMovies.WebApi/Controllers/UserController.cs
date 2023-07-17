using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WithMovies.Domain.Models;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [HttpPut]
        [Route("change-username")]
        private async Task<IActionResult> ChangeUsername(string newUsername)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier);
            if (username == null) return BadRequest("Invalid user");

            username = new Claim(ClaimTypes.NameIdentifier, newUsername);

            return Ok();
        }

        [HttpPut]
        [Route("change-email")]
        private async Task<IActionResult> ChangeEmail(User user, string newEmail)
        {
            if (user == null) return BadRequest("Invalid user");

            user.Email = newEmail;
            return Ok();
        }
    }
}
