using WithMovies.Domain.Enums;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;
using WithMovies.Domain.Enums;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace WithMovies.Business.UnitTests;

[Collection("Database Tests")]
public class ReviewServiceTests : UnitTestBase<IReviewService>
{
    private Movie _movie;
    private User _author;


    [Theory]
    [InlineData(1, "it was alright")]
    [InlineData(2, "it was okay")]
    [InlineData(3, "it was meh")]
    public async Task TestRead(int id, string expected)
    {
        var review = await _service.Read(id);

        Assert.Equal(review?.Message, expected);
    }

    [Fact]
    public async Task TestCreate()
    {         
        await _service.Create(_author, _movie, 3.5, "hoi", DateTime.Now);

        var reviewCheck = await _service.Read(4);

        Assert.Equal(reviewCheck.Author, _author);
    }

    // csharpier-ignore
    protected override Task SetupDatabase(DataContext context)
    {
        _movie = new Movie
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

        _author = new User
        {
            UserName = "Person",
            RecommendationProfile = new RecommendationProfile
            {
                GenreWeights = new float[20],
            },
            Friends = new List<User>() { },
            Watchlist = new List<Movie>() { },
            Reviews = new List<Review>() { },
            IsBlocked = false,
            CanReview = false,
        };

        context.Add(_movie);
        context.Add(_author);

        context.Add(new Review
        {
            Id = 1,
            Author = _author,
            Rating = 4.7,
            Message = "it was alright",
            PostedTime = new DateTime(2023, 07, 22),
            Movie = _movie,
        });

        context.Add(new Review
        {
            Id = 2,
            Author = _author,
            Rating = 4.3,
            Message = "it was okay",
            PostedTime = new DateTime(2023, 07, 22),
            Movie = _movie,
        });

        context.Add(new Review
        {
            Id = 3,
            Author = _author,
            Rating = 3.4,
            Message = "it was meh",
            PostedTime = new DateTime(2023, 07, 22),
            Movie = _movie,
        });

        return Task.CompletedTask;
    }
}
