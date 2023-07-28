using WithMovies.Domain.Interfaces;

namespace WithMovies.WebApi;

public class AlgorithmScheduler : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;

    public AlgorithmScheduler(IConfiguration configuration, IServiceProvider serviceProvider)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int minutes = int.Parse(_configuration["Algorithm:Interval"]!);

        var timer = new PeriodicTimer(TimeSpan.FromMinutes(minutes));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            var scope = _serviceProvider.CreateAsyncScope();
            var service = scope.ServiceProvider.GetRequiredService<IRecommendationService>();

            await service.RunRecommendationEngine();
        }
    }
}
