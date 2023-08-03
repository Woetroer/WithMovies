using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WithMovies.Business;
using WithMovies.Domain;
using WithMovies.Domain.Enums;
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
        private readonly IMovieCollectionService _movieCollectionService;
        private readonly DataContext _dataContext;

        public MovieController(
            IMovieService movieService,
            UserManager<User> userManager,
            IRecommendationService recommendationService,
            DataContext dataContext,
            IMovieCollectionService movieCollectionService
        )
        {
            _movieService = movieService;
            _userManager = userManager;
            _recommendationService = recommendationService;
            _dataContext = dataContext;
            _movieCollectionService = movieCollectionService;
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

        [HttpGet("collection/name/{name}")]
        public async Task<IActionResult> GetCollectionByNameAsync(string name)
        {
            var collection = await _movieCollectionService.MovieCollectionGetByNameAsync(name);

            if (collection == null)
                return NotFound();

            return Ok(
                new MovieCollectionDto
                {
                    Id = collection.Id,
                    BackdropPath = collection.BackdropPath,
                    PosterPath = collection.PosterPath,
                    Name = collection.Name,
                    ItemCount = collection.Movies.Count(),
                }
            );
        }

        public class SearchQueryInput
        {
            public string? RawText { get; set; }
            public required string[] Include { get; set; }
            public required string[] Exclude { get; set; }
            public required string[] IncludeGenres { get; set; }
            public required string[] ExcludeGenres { get; set; }
            public required string[] IncludeProductionCompanies { get; set; }
            public required string[] ExcludeProductionCompanies { get; set; }
            public string? Collection { get; set; }
            public required string SortMethod { get; set; }
            public required bool SortDescending { get; set; }

            public SearchQuery ToSearchQuery()
            {
                var query = new SearchQuery
                {
                    Text = RawText ?? "",
                    SortMethod = SortMethod switch
                    {
                        "release date" => Domain.SortMethod.ReleaseDate,
                        "rating" => Domain.SortMethod.Rating,
                        "relevance" => Domain.SortMethod.Relevance,
                        "popularity" => Domain.SortMethod.Popularity,
                        _ => throw new NotSupportedException(),
                    },
                    Adult = FilterState.None,
                    Filters = new(),
                    GenreFilters = new(),
                    SortDirection = SortDescending
                        ? SortDirection.Descending
                        : SortDirection.Ascending,
                    Collection = Collection,
                    ProductionCompanyFilters = new(),
                };

                foreach (var include in Include)
                {
                    if (include == "adult")
                        query.Adult = FilterState.Include;
                    else
                        query.Filters[include] = FilterState.Include;
                }

                foreach (var exclude in Exclude)
                {
                    if (exclude == "adult")
                        query.Adult = FilterState.Exclude;
                    else
                        query.Filters[exclude] = FilterState.Exclude;
                }

                foreach (var include in IncludeGenres)
                {
                    query.GenreFilters[Enum.Parse<Genre>(include)] = FilterState.Include;
                }

                foreach (var exclude in ExcludeGenres)
                {
                    query.GenreFilters[Enum.Parse<Genre>(exclude)] = FilterState.Exclude;
                }

                foreach (var include in IncludeProductionCompanies)
                {
                    query.ProductionCompanyFilters[include] = FilterState.Include;
                }

                foreach (var exclude in ExcludeProductionCompanies)
                {
                    query.ProductionCompanyFilters[exclude] = FilterState.Exclude;
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
