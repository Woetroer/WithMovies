using Microsoft.AspNetCore.Identity;
using WithMovies.Domain.Enums;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Business.UnitTests;

[Collection("Database Tests")]
public class UserServiceTests : UnitTestBase<IUserService>
{
    private bool[] _genrePreference;
    private User _user;
    private User _user2;

    [Fact]
    public async Task TestGetByName()
    {
        Assert.Equal(_user.UserName, _service.GetByName("Person").Result.UserName);
    }

    [Fact]
    public async Task TestGetAllCount()
    {
        var users = await _service.GetAllCount();
        Assert.Equal(2, users);
    }

    [Fact]
    public async Task TestAddPreferences()
    {
        _genrePreference = new bool[]
        {
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false,
            true,
            false
        };

        await _service.SetPreferencesAsync(_genrePreference, _user);

        Assert.Equal(_user.RecommendationProfile.ExplicitelyLikedGenres[0], true);
    }

    [Fact]
    public async Task TestBlock()
    {
        await _service.Block(_user);
        Assert.Equal(true, _user.IsBlocked);
    }

    [Fact]
    public async Task TestReviewRights()
    {
        await _service.ReviewRights(_user);
        Assert.Equal(true, _user.CanReview);
    }

    [Fact]
    public async Task TestDelete()
    {
        var name = _user.UserName;
        await _service.Delete(_user);
        Assert.Null(await _service.GetByName(name));
    }

    [Fact]
    public async Task TestOrderByReviews()
    {
        var users = await _service.MostActiveUsers(4);

        Assert.Equal(users.Count, 2);
    }

    [Fact]
    public async Task TestAverageReviews()
    {
        Assert.Equal(1.5, await _service.AverageReviewsPerUser());
    }

    [Fact]
    public async Task TestAddRole()
    {
        await _service.ApplyRole(_user);
        Assert.True(await _service.GetUserRole(_user));
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

        _user = new User
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
            RefreshToken = "refresh",
            RefreshTokenExpiry = DateTime.Now.AddHours(1)
        };

        _user2 = new User
        {
            UserName = "Person2",
            RecommendationProfile = new RecommendationProfile
            {
                GenreWeights = new float[20],
            },
            Friends = new List<User>() { },
            Watchlist = new List<Movie>() { },
            Reviews = new List<Review>() { },
            IsBlocked = false,
            CanReview = true,
            RefreshToken = "refresh",
            RefreshTokenExpiry = DateTime.Now.AddHours(1)
        };

        context.Add(movie);
        context.Add(_user);
        context.Add(_user2);


        context.Add(new Review
        {
            Id = 1,
            Author = _user,
            Rating = 4.7,
            Message = "it was alright",
            PostedTime = new DateTime(2023, 07, 22),
            Movie = movie,
        });

        context.Add(new Review
        {
            Id = 2,
            Author = _user,
            Rating = 4.3,
            Message = "it was okay",
            PostedTime = new DateTime(2023, 07, 22),
            Movie = movie,
        });

        context.Add(new Review
        {
            Id = 3,
            Author = _user,
            Rating = 3.4,
            Message = "it was meh",
            PostedTime = new DateTime(2023, 07, 22),
            Movie = movie,
        });

        return Task.CompletedTask;
    }
}
