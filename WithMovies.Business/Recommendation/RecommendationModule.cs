using WithMovies.Domain.Enums;
using WithMovies.Domain.Models;

namespace WithMovies.Business.Recommendation;

public abstract class RecommendationModule
{
    private float[] _genreWeights = new float[20];
    private Dictionary<int, float> _movieWeights = new();

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

    protected float GetMovieWeight(Movie movie) => GetMovieWeight(movie.Id);

    protected float GetMovieWeight(int id)
    {
        if (_movieWeights.TryGetValue(id, out float currValue))
            return currValue;

        return 0f;
    }

    protected void AddMovieWeight(Movie movie, float weight) => AddMovieWeight(movie.Id, weight);

    protected void AddMovieWeight(int id, float weight)
    {
        if (_movieWeights.TryGetValue(id, out float currValue))
            _movieWeights[id] = currValue + weight;
        else
            _movieWeights[id] = weight;
    }

    /// <summary>
    /// Set the weight of this movie explicitely, refrain from using this
    /// as it could interfere with other modules, try using <c>AddMovieWeight</c>
    /// as much as possible.
    /// </summary>
    /// <param name="movie">The movie to set the weight for</param>
    /// <param name="weight">The new weight</param>
    protected void SetMovieWeight(Movie movie, float weight) => SetMovieWeight(movie.Id, weight);

    /// <summary>
    /// Set the weight of this movie explicitely, refrain from using this
    /// as it could interfere with other modules, try using <c>AddMovieWeight</c>
    /// as much as possible.
    /// </summary>
    /// <param name="id">The id of the movie to set the weight for</param>
    /// <param name="weight">The new weight</param>
    protected void SetMovieWeight(int id, float weight)
    {
        _movieWeights[id] = weight;
    }

    public float[] GetGenreWeights() => this._genreWeights;

    public Dictionary<int, float> GetMovieWeights() => this._movieWeights;

    /// <summary>
    /// Reset this module, do not use this outside of <c>RecommendationEngine.Reset</c>
    /// </summary>
    public void Reset()
    {
        _genreWeights = new float[20];
        _movieWeights = new();
    }
}
