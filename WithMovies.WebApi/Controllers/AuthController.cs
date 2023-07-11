using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WithMovies.Domain.Models;
using WithMovies.WebApi.Models;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;

        public AuthController(UserManager<User> userManager, IConfiguration config, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _config = config;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            if (model == null) return BadRequest();
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password)) return Unauthorized();

            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id!)
                };


            foreach (var userRole in userRoles) claims.Add(new Claim(ClaimTypes.Role, userRole));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            await CheckIfValid(model);

            var userByUsername = await _userManager.FindByNameAsync(model.Username);
            if (userByUsername != null) return Conflict(new { Username = new List<string>() { "Username already exists" } });

            var userByEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userByEmail != null) return Conflict(new { Email = new List<string>() { "Email already exists" } });

            var user = new User
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Friends = new List<User>(),
                Watchlist = new List<Movie>(),
                Reviews = new List<Review>()
            };

            // Create user
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, string.Join(';', result.Errors.Select(x => x.Description)));

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.User))
                await _userManager.AddToRoleAsync(user, UserRoles.User);

            return Ok();
        }

        private async Task<IActionResult> CheckIfValid(Register model)
        {
            if (model == null) return BadRequest();

            var emailExists = await _userManager.FindByEmailAsync(model.Email!);
            if (emailExists != null)
                return StatusCode(StatusCodes.Status409Conflict, "User already exists");

            var userExists = await _userManager.FindByNameAsync(model.Username!);
            if (userExists != null)
                return StatusCode(StatusCodes.Status409Conflict, "User already exists");

            return Ok();
        }
    }
}
