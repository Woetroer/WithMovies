using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;
using WithMovies.Domain.Enums;

namespace WithMovies.Business.UnitTests;

[Collection("Database Tests")]
public class ReviewServiceTests : UnitTestBase<IReviewService>
{
    [Theory]
    [InlineData(1, "it was alright")]
    [InlineData(2, "it was okay")]
    [InlineData(3, "it was meh")]
    public async Task TestLink(int id, string expected)
    {
        var review = await _service.Read(id);

        Assert.Equal(review?.Message, expected);
    }

    // csharpier-ignore
    protected override Task SetupDatabase(DataContext context)
    {
        var movie = new Movie
        {
            Adult = false,
            Budget = 10000000,
            HomePage = null,
            ImdbId = "alright",
            OriginalLanguage = null,
            OriginalTitle = "The alright movie",
            Overview = "This movie is pretty alright",
            Popularity = 23.0,
            PosterPath = null,
            ReleaseDate = new(1976, 04, 15),
            Revenue = 100000000,
            Runtime = new(1, 17, 47),
            Status = MovieStatus.Released,
            Tagline = "it was alright",
            Title = "Alright!",
            VoteAverage = 4.6,
            VoteCount = 2,
            Genres = new List<Genre>() { Genre.Action },
            BelongsToCollection = null,
            ProductionCompanies = new List<ProductionCompany>(),
            ProductionCountries = new List<string?>() { "EN" },
            SpokenLanguages = new List<string?>() { "zhi" },
        };

        User author = new User
        {
            UserName = "Person",
            RecommendationProfile = new RecommendationProfile
            {
                GenreWeights = new float[20],
            },
            Friends = new List<User>() { },
            Watchlist = new List<Movie>() { },
            Reviews = new List<Review>() { }
        };

        context.Add(movie);
        context.Add(author);

        context.Add(new Review {
            Id = 1,
            Author = author,
            Rating = 4.7,
            Message = "it was alright",
            PostedTime = new DateTime(2023, 07, 22),
            Movie = movie,
        });

        context.Add(new Review {
            Id = 2,
            Author = author,
            Rating = 4.3,
            Message = "it was okay",
            PostedTime = new DateTime(2023, 07, 22),
            Movie = movie,
        });

        context.Add(new Review {
            Id = 3,
            Author = author,
            Rating = 3.4,
            Message = "it was meh",
            PostedTime = new DateTime(2023, 07, 22),
            Movie = movie,
        });

        return Task.CompletedTask;
    }
}
