using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WithMovies.Domain.Enums;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Business.Services;

class CreditsImport
{
    public required int Id { get; set; }
    public required ICollection<CrewMember> Crew { get; set; }
    public required ICollection<CastMember> Cast { get; set; }
}

public class CreditsService : ICreditsService
{
    private DataContext _dataContext;
    private IMovieService _movieService;
    private ILogger<ICreditsService> _logger;

    public CreditsService(
        DataContext dataContext,
        ILogger<ICreditsService> logger,
        IMovieService movieService
    )
    {
        _dataContext = dataContext;
        _logger = logger;
        _movieService = movieService;
    }

    public async Task ImportJsonAsync(Stream json)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new GenericEnumJsonConverter<Department>());

        var credits = (await JsonSerializer.DeserializeAsync<List<CreditsImport>>(json, options))!;

        // for progress bar
        double progress = 0.0;
        double step = 1.0 / (credits.Count - 1);
        int iteration = 0;

        // var movies = await _dataContext.Movies.ToDictionaryAsync(m => m.Id);
        var movies = new Dictionary<int, Movie>();

        foreach (var credit in credits)
        {
            progress += step;
            iteration++;

            if (iteration % 500 == 0)
            {
                string progressBar = $"|{new string('=', (int)(progress * 10.0)) + ">",-11}|";
                _logger.LogInformation($"{progressBar} Adding credits");
            }

            foreach (var member in credit.Cast)
            {
                member.Id = default;
            }

            foreach (var member in credit.Crew)
            {
                member.Id = default;
            }

            // await _dataContext.AddRangeAsync(credit.Cast);
            // await _dataContext.AddRangeAsync(credit.Crew);

            if (!movies.ContainsKey(credit.Id))
                movies[credit.Id] = (await _dataContext.Movies.FindAsync(credit.Id))!;

            var movie = movies[credit.Id];
            movie.Cast = credit.Cast;
            movie.Crew = credit.Crew;
            _dataContext.Update(movie);
        }
    }

    public async Task<List<CastMember>> CastGetFiveAsync()
    {
        List<CastMember> result = await _dataContext.CastMembers.ToListAsync();
        return result;
    }
}
