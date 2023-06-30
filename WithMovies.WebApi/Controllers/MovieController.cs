using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;
using WithMovies.WebApi.Extensions;
using WithMovies.WebApi.Dto;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            MovieDto movieToReturn = (await _movieService.MovieGetById(id)).ToDto();
            if (movieToReturn == null) { return NotFound(); }
            return Ok(movieToReturn);
        }
    }
}
