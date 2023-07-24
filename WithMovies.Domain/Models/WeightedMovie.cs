namespace WithMovies.Domain.Models;

public class WeightedMovie : BaseEntity
{
    /// <summary>
    /// The <c>RecommendationProfile</c> this <c>WeightedMovie</c> belongs to.
    /// </summary>
    public virtual RecommendationProfile Parent { get; set; } = null!;

    /// <summary>
    /// The movie this weight is targeting. This is using an idea instead of an
    /// object for optimization purposes
    /// </summary>
    public required int MovieId { get; set; }

    /// <summary>
    /// The weight of the movie, this value is computed in the algorithm. It
    /// is a float value ranging from <c>0.0</c> to <c>1.0</c>, where <c>0.0</c>
    /// means the user will dislike this movie, and <c>1.0</c> means the user
    /// will love it.
    /// </summary>
    public required float Weight { get; set; }
}
