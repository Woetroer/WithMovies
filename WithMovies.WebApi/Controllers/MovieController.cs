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
        public async Task<IActionResult> GetPreview()
        {
            List<PreviewDto> preview = (await _movieService.GetPreview()).ToPreviewDto();
            if (preview == null) { return NotFound(); }
            return Ok(preview);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movieToReturn = await _movieService.GetById(id);
            if (movieToReturn == null) { return NotFound(); }
            return Ok(movieToReturn.ToDto());
        }
    }
}
