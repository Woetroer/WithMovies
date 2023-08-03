namespace WithMovies.Domain.Interfaces;

public interface ISuggestionService
{
    public Task<ICollection<Suggestion>> SuggestKeywords(string text);
    public Task<ICollection<Suggestion>> SuggestProductionCompanies(string text);
    public Task<ICollection<Suggestion>> SuggestCollections(string text);
}
