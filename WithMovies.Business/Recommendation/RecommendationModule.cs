using WithMovies.Domain.Enums;
using WithMovies.Domain.Models;

namespace WithMovies.Business.Recommendation;

public abstract class RecommendationModule
{
    private float[] _genreWeights = new float[20];
    private Dictionary<int, float> _keywordWeights = new();

    public virtual void ProcessGenre(Genre genre, bool explicitelyLiked) { }

    public virtual void ProcessMovie(RecommendationProfileInput input) { }

    protected float GetGenreWeight(Genre genre) => _genreWeights[(int)genre];

    protected void AddGenreWeight(Genre genre, float weight)
    {
        _genreWeights[(int)genre] += weight;
    }

    /// <summary>
    /// Set the weight of this genre explicitely, refrain from using this
    /// as it could interfere with other modules, try using <c>AddGenreWeight</c>
    /// as much as possible.
    /// </summary>
    /// <param name="genre">The genre to set the weight for</param>
    /// <param name="weight">The new weight</param>
    protected void SetGenreWeight(Genre genre, float weight)
    {
        _genreWeights[(int)genre] = weight;
    }

    protected float GetKeywordWeight(Keyword keyword) => GetKeywordWeight(keyword.Id);

    protected float GetKeywordWeight(int id)
    {
        if (_keywordWeights.TryGetValue(id, out float currValue))
            return currValue;

        return 0f;
    }

    protected void AddKeywordWeight(Keyword keyword, float weight) =>
        AddKeywordWeight(keyword.Id, weight);

    protected void AddKeywordWeight(int id, float weight)
    {
        if (_keywordWeights.TryGetValue(id, out float currValue))
            _keywordWeights[id] = currValue + weight;
        else
            _keywordWeights[id] = weight;
    }

    /// <summary>
    /// Set the weight of this keyword explicitely, refrain from using this
    /// as it could interfere with other modules, try using <c>AddKeywordWeight</c>
    /// as much as possible.
    /// </summary>
    /// <param name="keyword">The keyword to set the weight for</param>
    /// <param name="weight">The new weight</param>
    protected void SetKeywordWeight(Keyword keyword, float weight) =>
        SetKeywordWeight(keyword.Id, weight);

    /// <summary>
    /// Set the weight of this keyword explicitely, refrain from using this
    /// as it could interfere with other modules, try using <c>AddMovieWeight</c>
    /// as much as possible.
    /// </summary>
    /// <param name="id">The id of the keyword to set the weight for</param>
    /// <param name="weight">The new weight</param>
    protected void SetKeywordWeight(int id, float weight)
    {
        _keywordWeights[id] = weight;
    }

    public float[] GetGenreWeights() => this._genreWeights;

    public Dictionary<int, float> GetKeywordWeights() => this._keywordWeights;

    /// <summary>
    /// Reset this module, do not use this outside of <c>RecommendationEngine.Reset</c>
    /// </summary>
    public void Reset()
    {
        _genreWeights = new float[20];
        _keywordWeights = new();
    }
}
