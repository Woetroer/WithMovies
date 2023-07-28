namespace WithMovies.Domain.Models;

public class WeightedKeywordId : BaseEntity
{
    public virtual required RecommendationProfile Parent { get; set; }
    public virtual required Keyword Keyword { get; set; }
    public required float Weight { get; set; }
}
