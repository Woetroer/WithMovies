using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithMovies.Business;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;
using static WithMovies.WebApi.Controllers.UserController;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMovieService _movieService;
        private readonly UserManager<User> _userManager;
        private DataContext _dataContext;
        public UserController(IUserService userService, IMovieService movieService, UserManager<User> userManager, DataContext dataContext)
        {
            _userService = userService;
            _movieService = movieService;
            _userManager = userManager;
            _dataContext = dataContext;
        }

        public record Profile(string Email, string Username);
        
        [HttpPost, Route("block"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleBlock(Profile profile)
        {
            var user = _userManager.Users.Where(x => x.Email == profile.Email && x.UserName == profile.Username).FirstOrDefault();
            string name = user.UserName;
            await _userService.Block(user);

            await _dataContext.SaveChangesAsync();
            return Ok("User " + name + " is blocked.");
        }

        [HttpPost, Route("reviewright"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleReviewRight(Profile profile)
        {
            var user = _userManager.Users.Where(x => x.Email == profile.Email && x.UserName == profile.Username).FirstOrDefault();
            string name = user.UserName;


            await _userService.ReviewRights(user);
            await _dataContext.SaveChangesAsync();

            if (user.CanReview = false)
                return Ok("User " + user + " can no longer review.");
            return Ok("User " + user + " can review once again.");
        }

        [HttpDelete, Route("delete"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Profile profile)
        {
            var user = _userManager.Users.Where(x => x.Email == profile.Email && x.UserName == profile.Username).FirstOrDefault();
            string name = user.UserName;

            await _userService.Delete(user);
            await _dataContext.SaveChangesAsync();

            return Ok("User" + name + "is deleted.");
        }

    }
}
