﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : MyControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMovieService _movieService;
        private readonly UserManager<User> _userManager;

        public UserController(
            IUserService userService,
            IMovieService movieService,
            UserManager<User> userManager
        )
        {
            _userService = userService;
            _movieService = movieService;
            _userManager = userManager;
        }

        [HttpPut("change-username/{newUsername}"), Authorize]
        public async Task<IActionResult> ChangeUsername(string newUsername)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
                return NotFound();

            if (user.UserName == newUsername)
                return Conflict("The new username should be unique");

            user.UserName = newUsername;

            await _userManager.UpdateAsync(user);

            return Ok();
        }

        [HttpPut("change-email/{newEmail}"), Authorize]
        public async Task<IActionResult> ChangeEmail(string newEmail)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
                return NotFound();

            if (user.Email == newEmail)
                return Conflict("The new email should be unique");

            user.Email = newEmail;

            await _userManager.UpdateAsync(user);

            return Ok();
        }

        public record Profile(string Email, string Username);

        [HttpPost, Route("block"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleBlock(Profile profile)
        {
            var user = _userManager.Users
                .Where(x => x.Email == profile.Email && x.UserName == profile.Username)
                .FirstOrDefault();

            if (user == null)
                return NotFound();

            await _userService.Block(user);

            return Ok();
        }

        [HttpPost, Route("reviewright"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleReviewRight(Profile profile)
        {
            var user = _userManager.Users.FirstOrDefault(
                x => x.Email == profile.Email && x.UserName == profile.Username
            );

            if (user == null)
                return NotFound();

            await _userService.ReviewRights(user);

            return Ok();
        }

        [HttpDelete, Route("delete"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Profile profile)
        {
            var user = _userManager.Users
                .Where(x => x.Email == profile.Email && x.UserName == profile.Username)
                .FirstOrDefault();

            if (user == null)
                return NotFound();

            await _userService.Delete(user);

            return Ok();
        }
    }
}
