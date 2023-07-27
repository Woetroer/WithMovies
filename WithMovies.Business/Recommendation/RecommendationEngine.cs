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
            __inputs ??= _target!.Inputs.ToArray();

            return __inputs;
        }
    }

    private float[] _genreWeights = new float[20];
    private Dictionary<int, float> _keywordWeights = new();

    private DataContext _dataContext;

    public RecommendationEngine(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

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
        _target.KeywordWeights = _keywordWeights
            .Select(
                (pair, i) =>
                    new WeightedKeywordId
                    {
                        Keyword = _dataContext.Keywords.Find(pair.Key)!,
                        Weight = pair.Value
                    }
            )
            .ToList();
    }

    private void Reset()
    {
        _genreWeights = new float[20];
        _keywordWeights = new();

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

            var movieWeights = module.GetKeywordWeights();

            foreach (var pair in movieWeights)
            {
                if (_keywordWeights.TryGetValue(pair.Key, out float currValue))
                    _keywordWeights[pair.Key] = currValue + pair.Value;
                else
                    _keywordWeights[pair.Key] = pair.Value;
            }
        }

        var max = _genreWeights.Append(0.01f).Max();
        for (int i = 0; i < _genreWeights.Length; i++)
        {
            _genreWeights[i] = _genreWeights[i] / max;
        }

        max = _keywordWeights.Values.Append(0.01f).Max();
        foreach (var pair in _keywordWeights)
        {
            _keywordWeights[pair.Key] /= max;
        }
    }
}
