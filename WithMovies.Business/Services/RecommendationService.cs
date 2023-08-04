using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WithMovies.Business.Recommendation;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Business.Services;

public class RecommendationService : IRecommendationService
{
    private ILogger<IRecommendationService> _logger;
    private DataContext _dataContext;

    public RecommendationService(ILogger<IRecommendationService> logger, DataContext dataContext)
    {
        _logger = logger;
        _dataContext = dataContext;
    }

    public async Task RunRecommendationEngine()
    {
        _logger.LogInformation("Running recommendation engine");

        var engine = new RecommendationEngine(_dataContext);

        engine.Modules.Add(new ExplicitelyLikedGenreModule());
        engine.Modules.Add(new VisitedKeywordsModule());
        engine.Modules.Add(new ReviewedMoviesKeywordsModule());

        var users = _dataContext.Users.Where(u => u.LastLogin > DateTime.Now.AddDays(-1));

        var startTime = DateTime.Now;

        foreach (var user in users)
        {
            _logger.LogInformation($"Running for {user.NormalizedUserName}");

            engine.SetTarget(user.RecommendationProfile);
            engine.Run();
        }

        await _dataContext.SaveChangesAsync();

        var duration = DateTime.Now - startTime;

        _logger.LogInformation($"Recommendation engine run took {duration.TotalSeconds:N4}s");
    }

    public Task FlagViewedDetailsPageAsync(User user, Movie movie)
    {
        var profile = user.RecommendationProfile;
        var input = profile.Inputs
            .AsQueryable()
            .Include(i => i.Movie)
            .FirstOrDefault(i => i.Movie.Id == movie.Id);

        if (input == null)
        {
            profile.Inputs.Add(
                new RecommendationProfileInput
                {
                    Movie = movie,
                    Parent = profile,
                    Rating = null,
                    Created = DateTime.Now,
                    ViewedDetailsPage = true,
                    Watched = false,
                }
            );
            _dataContext.Update(profile);
        }
        else
        {
            input.ViewedDetailsPage = true;
            _dataContext.Update(input);
        }

        return Task.CompletedTask;
    }

    public Task FlagReviewedMovieAsync(User user, Movie movie, double rating)
    {
        var profile = user.RecommendationProfile;
        var input = profile.Inputs
            .AsQueryable()
            .Include(i => i.Movie)
            .FirstOrDefault(i => i.Movie.Id == movie.Id);

        if (input == null)
        {
            profile.Inputs.Add(
                new RecommendationProfileInput
                {
                    Movie = movie,
                    Parent = profile,
                    Rating = rating,
                    Created = DateTime.Now,
                    ViewedDetailsPage = false,
                    Watched = true,
                }
            );
            _dataContext.Update(profile);
        }
        else
        {
            input.Rating = rating;
            input.Watched = true;
            _dataContext.Update(input);
        }

        return Task.CompletedTask;
    }
}
