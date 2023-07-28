using WithMovies.Domain.Models;

namespace WithMovies.Business.Recommendation;

public class VisitedKeywordsModule : RecommendationModule
{
    public override void ProcessMovie(RecommendationProfileInput input)
    {
        if (input.ViewedDetailsPage)
        {
            foreach (var keyword in input.Movie.Keywords)
            {
                if (keyword.Name == "duringcreditsstinger" || keyword.Name == "aftercreditsstinger")
                    continue;

                AddKeywordWeight(keyword, 0.1f);
            }
        }
    }
}
