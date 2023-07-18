using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithMovies.Business;
using WithMovies.Domain.Models;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : MyControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _dataContext;
        public UserController(UserManager<User> userManager, DataContext dataContext)
        {
            _userManager = userManager;
            _dataContext = dataContext;
        }
        [HttpPut]
        [Route("change-username")]
        private async Task<IActionResult> ChangeUsername(string newUsername)
        {
            User? user = await _userManager.FindByNameAsync(UserId);
            if (user == null) return BadRequest("Invalid user");
            if (user.UserName == newUsername) return BadRequest("The new username should be unique");

            user.UserName = newUsername;

            await _dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Route("change-email")]
        private async Task<IActionResult> ChangeEmail(string newEmail)
        {
            User? user = await _userManager.FindByNameAsync(UserId);
            if (user == null) return BadRequest("Invalid user");
            if (user.Email == newEmail) return BadRequest("The new email should be unique");

            user.Email = newEmail;

            await _dataContext.SaveChangesAsync();

            return Ok();
        }
    }
}
