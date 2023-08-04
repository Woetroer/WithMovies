using WithMovies.Domain.Models;

namespace WithMovies.Business.Recommendation;

public class ReviewedMoviesKeywordsModule : RecommendationModule
{
    public override void ProcessMovie(RecommendationProfileInput input)
    {
        if (input.Rating != null && input.Rating > 2.5)
        {
            foreach (var keyword in input.Movie.Keywords)
            {
                if (keyword.Name == "duringcreditsstinger" || keyword.Name == "aftercreditsstinger")
                    continue;

                AddKeywordWeight(keyword, (float)input.Rating! / 2.5f);
            }
        }
    }
}
