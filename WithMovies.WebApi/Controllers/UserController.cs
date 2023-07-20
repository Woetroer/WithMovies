using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithMovies.Domain.Models;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : MyControllerBase
    {
        private readonly UserManager<User> _userManager;
        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPut]
        [Route("change-username")]
        public async Task<IActionResult> ChangeUsername(string newUsername)
        {
            User? user = await _userManager.FindByIdAsync(UserId);
            if (user == null) return BadRequest("Invalid user");
            if (user.UserName == newUsername) return BadRequest("The new username should be unique");

            user.UserName = newUsername;

            await _userManager.UpdateAsync(user);

            return Ok();
        }

        [HttpPut]
        [Route("change-email")]
        public async Task<IActionResult> ChangeEmail(string newEmail)
        {
            User? user = await _userManager.FindByIdAsync(UserId);
            if (user == null) return BadRequest("Invalid user");
            if (user.Email == newEmail) return BadRequest("The new email should be unique");

            user.Email = newEmail;

            await _userManager.UpdateAsync(user);

            return Ok();
        }

    }
}
