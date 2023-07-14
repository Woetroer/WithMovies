using Microsoft.AspNetCore.Mvc;
using WithMovies.Domain.Interfaces;
using WithMovies.WebApi.Dtos;
using WithMovies.WebApi.Extensions;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [Route("GetPreview")]
        [HttpGet]
        public async Task<IActionResult> GetPopularMovies()
        {
            List<PreviewDto> preview = (await _movieService.GetPopularMovies()).ToPreviewDto();
            return Ok(preview);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            MovieDto movieToReturn = (await _movieService.GetById(id)).ToDto();
            if (movieToReturn == null) { return NotFound(); }
            return Ok(movieToReturn);
        }
    }
}
