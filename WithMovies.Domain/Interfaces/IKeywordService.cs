using WithMovies.Domain.Models;

namespace WithMovies.Domain.Interfaces
{
    public interface IKeywordService
    {
        Task ImportJsonAsync(Stream json);
        Task<List<Keyword>> FindKeywordGroups(string text);
        Task<List<KeywordSuggestion>> FindKeywordSuggestions(string text);
    }
}
