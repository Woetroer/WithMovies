namespace WithMovies.Domain.Models;

public class RecommendationProfileInput : BaseEntity
{
    /// <summary>
    /// The RecommendationProfile this input node belongs to
    /// </summary>
    public virtual RecommendationProfile Parent { get; set; } = null!;

    /// <summary>
    /// The movie this input node is talking about
    /// </summary>
    public virtual Movie Movie { get; set; } = null!;

    /// <summary>
    /// The given rating by the user if present, otherwise <c>null</c>
    /// </summary>
    public double? Rating { get; set; } = null;

    /// <summary>
    /// Has the user watched this movie
    /// </summary>
    public bool Watched { get; set; } = false;

    /// <summary>
    /// Has the user viewed the details page for this movie (<c>/movie/:id</c>)
    /// </summary>
    public bool ViewedDetailsPage { get; set; } = false;

    /// <summary>
    /// The time this RecommendationProfileInput node was created
    /// </summary>
    public DateTime Created { get; set; } = DateTime.Now;
}
