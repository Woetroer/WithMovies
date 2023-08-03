using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WithMovies.Business;
using WithMovies.Domain;
using WithMovies.Domain.Interfaces;

namespace WithMovies.Data.Sqlite;

public class SqliteSuggestionService : ISuggestionService
{
    private DataContext _dataContext;
    private ILogger<ISuggestionService> _logger;

    public SqliteSuggestionService(DataContext dataContext, ILogger<ISuggestionService> logger)
    {
        _dataContext = dataContext;
        _logger = logger;
    }

    public async Task<ICollection<Suggestion>> SuggestCollections(string text)
    {
        await _dataContext.LoadExtension("fuzzy").Database.BeginTransactionAsync();

        var suggestions = await _dataContext
            .Set<KeywordRecord>()
            .FromSqlRaw(
                """
                SELECT DISTINCT Name,
                    jaro_winkler(translit(LOWER(Name)), translit(LOWER(:text))) AS Weight
                FROM MovieCollections
                WHERE Weight > 0.5
                ORDER BY Weight DESC
                LIMIT 25
                """,
                new SqliteParameter("text", text)
            )
            .ToListAsync();

        return suggestions.Select(k => new Suggestion(k.Name!, text, (float)k.Weight!)).ToList();
    }

    public async Task<ICollection<Suggestion>> SuggestKeywords(string text)
    {
        await _dataContext.LoadExtension("fuzzy").Database.BeginTransactionAsync();

        var suggestions = await _dataContext
            .Set<KeywordRecord>()
            .FromSqlRaw(
                """
                SELECT DISTINCT Name,
                    jaro_winkler(translit(Name), translit(:text)) AS Weight
                FROM Keywords
                WHERE Weight > 0.85
                ORDER BY Weight DESC
                LIMIT 25
                """,
                new SqliteParameter("text", text)
            )
            .ToListAsync();

        return suggestions.Select(k => new Suggestion(k.Name!, text, (float)k.Weight!)).ToList();
    }

    public async Task<ICollection<Suggestion>> SuggestProductionCompanies(string text)
    {
        await _dataContext.LoadExtension("fuzzy").Database.BeginTransactionAsync();

        var suggestions = await _dataContext
            .Set<KeywordRecord>()
            .FromSqlRaw(
                """
                SELECT DISTINCT Name,
                    jaro_winkler(translit(LOWER(Name)), translit(LOWER(:text))) AS Weight
                FROM ProductionCompanies
                WHERE Weight > 0.5
                ORDER BY Weight DESC
                LIMIT 25
                """,
                new SqliteParameter("text", text)
            )
            .ToListAsync();

        return suggestions.Select(k => new Suggestion(k.Name!, text, (float)k.Weight!)).ToList();
    }
}
