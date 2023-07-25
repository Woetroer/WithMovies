using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithMovies.Domain.Models;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : MyControllerBase
    {
        private readonly UserManager<User> _userManager;
        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPut("change-username/{newUsername}"), Authorize]
        public async Task<IActionResult> ChangeUsername(string newUsername)
        {
            if (newUsername == null) return BadRequest("The new username cannot be empty");
            User? user = await _userManager.FindByIdAsync(UserId);
            if (user == null) return BadRequest("User Not Found!");
            if (user.UserName == newUsername) return BadRequest("The new username should be unique");

            user.UserName = newUsername;

            await _userManager.UpdateAsync(user);

            return Ok();
        }

        [HttpPut("change-email/{newEmail}"), Authorize]
        public async Task<IActionResult> ChangeEmail(string newEmail)
        {
            if (newEmail == null) return BadRequest("The new email cannot be empty");
            User? user = await _userManager.FindByIdAsync(UserId);
            if (user == null) return BadRequest("User Not Found!");
            if (user.Email == newEmail) return BadRequest("The new email should be unique");

            user.Email = newEmail;

            await _userManager.UpdateAsync(user);

            return Ok();
        }

    }
}
