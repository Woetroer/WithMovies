using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WithMovies.Business;
using WithMovies.Domain.Models;
using WithMovies.WebApi.Models;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : MyControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _dataContext;
        private readonly IConfiguration _config;

        public AuthController(
            UserManager<User> userManager,
            IConfiguration config,
            RoleManager<IdentityRole> roleManager,
            DataContext dataContext
        )
        {
            _userManager = userManager;
            _config = config;
            _roleManager = roleManager;
            _dataContext = dataContext;
        }

        public class AuthenticatedResponse
        {
            public string? AccessToken { get; set; }
            public string? RefreshToken { get; set; }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (model == null)
                return BadRequest();

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized();

            var accessToken = await GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddDays(7);

            user.LastLogin = DateTime.Now;
            await _dataContext.SaveChangesAsync();

            return Ok(
                new AuthenticatedResponse { AccessToken = accessToken, RefreshToken = refreshToken }
            );
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            await CheckIfValid(model);

            var userByUsername = await _userManager.FindByNameAsync(model.Username);
            if (userByUsername != null)
            {
                return Conflict(
                    new { Username = new List<string>() { "Username already exists" } }
                );
            }

            var userByEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userByEmail != null)
                return Conflict(new { Email = new List<string>() { "Email already exists" } });

            var user = new User
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Friends = new List<User>(),
                Watchlist = new List<Movie>(),
                Reviews = new List<Review>(),
                LastLogin = DateTime.Now,
                RecommendationProfile = new RecommendationProfile
                {
                    Inputs = new List<RecommendationProfileInput>(),
                    ExplicitelyLikedGenres = new bool[20],
                    KeywordWeights = new List<WeightedKeywordId>(),
                    GenreWeights = new float[20],
                },
                IsBlocked = false,
                CanReview = true,
                RefreshToken = "",
                RefreshTokenExpiry = DateTime.Now
            };

            // Create user
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    string.Join(';', result.Errors.Select(x => x.Description))
                );
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.User))
                await _userManager.AddToRoleAsync(user, UserRoles.User);

            return Ok();
        }

        private async Task<IActionResult> CheckIfValid(RegisterModel model)
        {
            if (model == null)
                return BadRequest();

            var emailExists = await _userManager.FindByEmailAsync(model.Email!);
            if (emailExists != null)
                return StatusCode(StatusCodes.Status409Conflict, "User already exists");

            var userExists = await _userManager.FindByNameAsync(model.Username!);
            if (userExists != null)
                return StatusCode(StatusCodes.Status409Conflict, "User already exists");

            return Ok();
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel.RefreshToken == null)
                return BadRequest("Refresh token is null!");
            if (tokenApiModel.AccessToken == null)
                return BadRequest("Access Token is null!");

            string refreshToken = tokenApiModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(tokenApiModel.AccessToken);
            var username = principal.Identity!.Name;

            var user = _userManager.Users.SingleOrDefault(u => u.UserName == username);
            if (
                user is null
                || user.RefreshToken != refreshToken
                || user.RefreshTokenExpiry <= DateTime.Now
            )
                return BadRequest("Invalid client request");

            if (user.RefreshToken != refreshToken)
                return BadRequest("Invalid refresh token!");
            if (user.RefreshTokenExpiry < DateTime.Now)
                return BadRequest("The refresh token is expired!");

            var newRefreshToken = GenerateRefreshToken();
            var newAccessToken = await GenerateAccessToken(user);

            user.RefreshToken = newRefreshToken;
            await _dataContext.SaveChangesAsync();

            return Ok(
                new AuthenticatedResponse()
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                }
            );
        }

        [HttpPost, Authorize]
        [Route("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity!.Name;
            User? user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
                return BadRequest();

            user.RefreshToken = string.Empty;
            await _dataContext.SaveChangesAsync();
            return Ok();
        }

        private async Task<string> GenerateAccessToken(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id!)
            };

            foreach (var userRole in userRoles)
                claims.Add(new Claim(ClaimTypes.Role, userRole));

            var authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var tokeOptions = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(5),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    authSigningKey,
                    SecurityAlgorithms.HmacSha256
                )
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
                ),
                ValidateLifetime = false // here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters,
                out SecurityToken securityToken
            );
            if (
                securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase
                )
            )
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}
