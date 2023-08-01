using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WithMovies.Domain.Interfaces;

namespace WithMovies.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KeywordController : ControllerBase
{
    private IKeywordService _keywordService;

    public KeywordController(IKeywordService keywordService)
    {
        _keywordService = keywordService;
    }

    [HttpGet("suggest/{query}")]
    public async Task<IActionResult> FindSuggestions(string query)
    {
        return Ok(await _keywordService.FindKeywordSuggestions(query));
    }
}
