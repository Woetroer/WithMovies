using WithMovies.Domain.Models;

namespace WithMovies.Domain.Interfaces;

public interface IRecommendationService
{
    public Task RunRecommendationEngine();

    public Task FlagViewedDetailsPageAsync(User user, Movie movie);
}
