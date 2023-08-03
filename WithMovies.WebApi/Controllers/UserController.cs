﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WithMovies.Business;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;
using WithMovies.WebApi.Dtos;
using WithMovies.WebApi.Extensions;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : MyControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMovieService _movieService;
        private readonly UserManager<User> _userManager;
        private DataContext _dataContext;

        public UserController(
            IUserService userService,
            IMovieService movieService,
            UserManager<User> userManager,
            DataContext dataContext
        )
        {
            _userService = userService;
            _movieService = movieService;
            _userManager = userManager;
            _dataContext = dataContext;
        }

        [HttpPut("change-username/{newUsername}"), Authorize]
        public async Task<IActionResult> ChangeUsername(string newUsername)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
                return NotFound();

            if (user.UserName == newUsername)
                return Conflict("The new username should be unique");

            var userByUsername = await _userManager.FindByNameAsync(newUsername);
            if (userByUsername != null)
                return Conflict("Username already exists");

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

            var userByEmail = await _userManager.FindByEmailAsync(newEmail);
            if (userByEmail != null)
                return Conflict("Email already exists");

            user.Email = newEmail;

            await _userManager.UpdateAsync(user);

            return Ok();
        }

        public record Profile(string Email, string Username);

        [HttpPost("block"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleBlock(Profile profile)
        {
            var user = _userManager.Users
                .Where(x => x.Email == profile.Email && x.UserName == profile.Username)
                .FirstOrDefault();

            if (user == null)
                return NotFound();

            await _userService.Block(user);

            await _dataContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("reviewright"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleReviewRight(Profile profile)
        {
            var user = _userManager.Users.FirstOrDefault(
                x => x.Email == profile.Email && x.UserName == profile.Username
            );

            if (user == null)
                return NotFound();

            await _userService.ReviewRights(user);
            await _dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("delete"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Profile profile)
        {
            var user = _userManager.Users
                .Where(x => x.Email == profile.Email && x.UserName == profile.Username)
                .FirstOrDefault();

            if (user == null)
                return NotFound();

            await _userService.Delete(user);
            await _dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("users-most-reviews/{amount}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UsersWithMostReviews(int amount)
        {
            var users = await _userService.MostActiveUsers(amount);
            return Ok(users.Select(UserExtensions.ToDto));
        }

        [HttpGet("avg-reviews"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> AverageReviews()
        {
            return Ok(await _userService.AverageReviewsPerUser());
        }

        [HttpGet("all-users"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersCount()
        {
            return Ok(await _userService.GetAllCount());
        }
    }
}
