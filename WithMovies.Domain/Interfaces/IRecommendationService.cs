using WithMovies.Domain.Models;

namespace WithMovies.Domain.Interfaces;

public interface IRecommendationService
{
    public Task RunRecommendationEngine();

    public Task FlagViewedDetailsPageAsync(User user, Movie movie);
    public Task FlagReviewedMovieAsync(User user, Movie movie, double rating);
}
