using Microsoft.Extensions.Logging;
using WithMovies.Business.Recommendation;
using WithMovies.Domain.Interfaces;

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

        var engine = new RecommendationEngine();
        engine.Modules.Add(new ExplicitelyLikedGenreModule());

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
}
