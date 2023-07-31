using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WithMovies.Business;
using WithMovies.Domain;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;
using WithMovies.WebApi.Dtos;
using WithMovies.WebApi.Extensions;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : MyControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly UserManager<User> _userManager;
        private readonly IRecommendationService _recommendationService;
        private readonly DataContext _dataContext;

        public MovieController(
            IMovieService movieService,
            UserManager<User> userManager,
            IRecommendationService recommendationService,
            DataContext dataContext
        )
        {
            _movieService = movieService;
            _userManager = userManager;
            _recommendationService = recommendationService;
            _dataContext = dataContext;
        }

        [HttpGet("trending/{start}/{limit}")]
        public async Task<IActionResult> Trending(int start, int limit) =>
            Ok(
                (await _movieService.GetTrending(start, limit)).Select(MovieExtensions.ToPreviewDto)
            );

        [HttpGet("trending/recommended/{start}/{limit}"), Authorize]
        public async Task<IActionResult> TrendingRecommended(int start, int limit)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
                return Unauthorized();

            return Ok(
                (await _movieService.GetTrending(user, start, limit)).Select(
                    MovieExtensions.ToPreviewDto
                )
            );
        }

        [HttpGet("linked/friends/{start}/{limit}"), Authorize]
        public async Task<IActionResult> LinkedFriends(int start, int limit)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
                return Unauthorized();

            return Ok(
                (await _movieService.GetFriendMovies(user))
                    .Skip(start)
                    .Take(limit)
                    .Select(MovieExtensions.ToPreviewDto)
            );
        }

        [HttpGet("linked/watchlist/{start}/{limit}"), Authorize]
        public async Task<IActionResult> LinkedWatchList(int start, int limit)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
                return Unauthorized();

            return Ok(
                (await _movieService.GetWatchList(user))
                    .Skip(start)
                    .Take(limit)
                    .Select(MovieExtensions.ToPreviewDto)
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);

            if (movie == null)
                return NotFound();

            return Ok(movie.ToDto());
        }

        [HttpGet("{id}/authorized"), Authorize]
        public async Task<IActionResult> GetByIdAuthorized(int id)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            if (user == null)
                return Unauthorized();

            var movie = await _movieService.GetByIdAsync(id);

            if (movie == null)
                return NotFound();

            await _recommendationService.FlagViewedDetailsPageAsync(user, movie);
            await _dataContext.SaveChangesAsync();

            return Ok(movie.ToDto());
        }

        public class SearchQueryInput
        {
            public required string RawText { get; set; }
            public required string[] Include { get; set; }
            public required string[] Exclude { get; set; }
            public required string SortMethod { get; set; }
            public required bool SortDescending { get; set; }

            public SearchQuery ToSearchQuery()
            {
                var query = new SearchQuery
                {
                    Text = RawText,
                    SortMethod = SortMethod switch
                    {
                        "release date" => Domain.SortMethod.ReleaseDate,
                        "rating" => Domain.SortMethod.Rating,
                        "relevance" => Domain.SortMethod.Relevance,
                        "popularity" => Domain.SortMethod.Popularity,
                        _ => throw new NotSupportedException(),
                    },
                    Adult = FilterState.None,
                    SortDirection = SortDescending
                        ? SortDirection.Descending
                        : SortDirection.Ascending,
                };

                foreach (var include in Include)
                {
                    if (include == "adult")
                        query.Adult = FilterState.Include;
                }

                foreach (var exclude in Exclude)
                {
                    if (exclude == "adult")
                        query.Adult = FilterState.Exclude;
                }

                return query;
            }
        };

        [HttpPost("query/{start}/{limit}")]
        public async Task<IActionResult> QueryMovies(
            SearchQueryInput queryInput,
            int start,
            int limit
        )
        {
            SearchQuery query;

            try
            {
                query = queryInput.ToSearchQuery();
            }
            catch (NotSupportedException)
            {
                return BadRequest();
            }

            return Ok(await _movieService.Query(query, start, limit));
        }
    }
}
