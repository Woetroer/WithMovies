using WithMovies.Domain.Enums;
using WithMovies.Domain.Models;

namespace WithMovies.Business.Recommendation;

public class RecommendationEngine
{
    private RecommendationProfile? _target;
    public List<RecommendationModule> Modules { get; set; } = new();

    // Proxies to prevent EFCore from repeatedly querying the database
    private RecommendationProfileInput[]? __inputs = null;
    private RecommendationProfileInput[] _inputs
    {
        get
        {
            // If the inputs have been queried before, use the last results
            if (__inputs == null)
                __inputs = _target!.Inputs.ToArray();

            return __inputs;
        }
    }

    private float[] _genreWeights = new float[20];
    private Dictionary<int, float> _movieWeights = new();

    public void SetTarget(RecommendationProfile target)
    {
        __inputs = null;
        _target = target;
    }

    public void Run()
    {
        if (_target == null)
            throw new NullReferenceException();

        Reset();

        foreach (var module in Modules)
        {
            for (int i = 0; i < 20; i++)
            {
                module.ProcessGenre((Genre)i, _target.ExplicitelyLikedGenres[i]);
            }

            foreach (var input in _inputs)
            {
                module.ProcessMovie(input);
            }
        }

        MergeOutputs();

        _target.GenreWeights = _genreWeights;
        _target.MovieWeights = _movieWeights
            .Select(
                (pair, i) =>
                    new WeightedMovie
                    {
                        MovieId = pair.Key,
                        Parent = _target,
                        Weight = pair.Value
                    }
            )
            .ToList();
    }

    private void Reset()
    {
        _genreWeights = new float[20];
        _movieWeights = new();

        foreach (var module in Modules)
            module.Reset();
    }

    private void MergeOutputs()
    {
        foreach (var module in Modules)
        {
            var genreWeights = module.GetGenreWeights();

            for (int i = 0; i < genreWeights.Length; i++)
                _genreWeights[i] += genreWeights[i];

            var movieWeights = module.GetMovieWeights();

            foreach (var pair in movieWeights)
            {
                if (_movieWeights.TryGetValue(pair.Key, out float currValue))
                    _movieWeights[pair.Key] = currValue + pair.Value;
                else
                    _movieWeights[pair.Key] = pair.Value;
            }
        }

        var max = _genreWeights.Max();
        for (int i = 0; i < _genreWeights.Length; i++)
        {
            _genreWeights[i] = _genreWeights[i] / max;
        }

        max = _movieWeights.Values.Max();
        foreach (var pair in _movieWeights)
        {
            _movieWeights[pair.Key] /= max;
        }
    }
}
