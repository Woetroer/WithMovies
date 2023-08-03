using Microsoft.AspNetCore.Mvc;
using WithMovies.Domain.Interfaces;

namespace WithMovies.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestionController : ControllerBase
    {
        private ISuggestionService _suggestionService;

        public SuggestionController(ISuggestionService suggestionService)
        {
            _suggestionService = suggestionService;
        }

        [HttpGet("keywords/{text}")]
        public async Task<IActionResult> SuggestKeywords(string text)
        {
            return Ok(await _suggestionService.SuggestKeywords(text));
        }

        [HttpGet("collections/{text}")]
        public async Task<IActionResult> SuggestCollections(string text)
        {
            return Ok(await _suggestionService.SuggestCollections(text));
        }

        [HttpGet("companies/{text}")]
        public async Task<IActionResult> SuggestCompanies(string text)
        {
            return Ok(await _suggestionService.SuggestProductionCompanies(text));
        }
    }
}
