using WithMovies.Domain.Models;

namespace WithMovies.Domain.Interfaces
{
    public interface IKeywordService
    {
        Task ImportJsonAsync(Stream json);
        Task<IQueryable<Keyword>> FindKeywords(string[] names);
    }
}
