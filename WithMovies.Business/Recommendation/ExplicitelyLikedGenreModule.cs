using WithMovies.Domain.Enums;

namespace WithMovies.Business.Recommendation;

public class ExplicitelyLikedGenreModule : RecommendationModule
{
    public override void ProcessGenre(Genre genre, bool explicitelyLiked)
    {
        AddGenreWeight(genre, explicitelyLiked ? 1.0f : 0.0f);
    }
}
