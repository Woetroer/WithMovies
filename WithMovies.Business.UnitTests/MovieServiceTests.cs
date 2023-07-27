using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WithMovies.Domain.Enums;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Business.UnitTests
{
    [Collection("Database Tests")]
    public class MovieServiceTests : UnitTestBase<IMovieService>
    {
        [Theory]
        [InlineData(3, "Fourth time's the charm...")]
        [InlineData(2, "The Third!")]
        [InlineData(5, "5 pees in a pod")]
        public async Task GetById(int id, string expected)
        {
            var movie = await _service.GetById(id);

            Assert.Equal(movie?.Title, expected);
        }

        protected override Task SetupDatabase(DataContext context)
        {
            var testMovie1 = new Movie
            {
                Keywords = new List<Keyword>(),
                Id = 4,
                ImdbId = "movie1",
                BelongsToCollection = null,
                Title = "First Flight",
                Tagline = "Lets test!",
                OriginalLanguage = "english",
                OriginalTitle = "TestMovie1",
                Adult = true,
                Overview = "First Tester",
                Budget = 1200000,
                Genres = new List<Genre>() { Genre.Adventure, Genre.Mystery },
                HomePage = "HomePage1",
                PosterPath = "PosertPage1",
                ProductionCompanies = new List<ProductionCompany>(),
                ProductionCountries = new List<string?>() { "EN" },
                ReleaseDate = new DateTime(),
                Revenue = 20000000,
                Runtime = new TimeSpan(),
                SpokenLanguages = new List<string?>() { "en" },
                Status = MovieStatus.Released,
                VoteAverage = 3.10,
                VoteCount = 200,
                Popularity = 4.23,
                Cast = new List<CastMember>() { },
                Crew = new List<CrewMember>() { },
                Reviews = new List<Review>() { }
            };
            context.Add(testMovie1);

            context.Add(
                new Movie
                {
                    Id = 1,
                    ImdbId = "movie2",
                    BelongsToCollection = null,
                    Title = "Electric Boogalu!",
                    Tagline = "The second.",
                    OriginalLanguage = "indian",
                    OriginalTitle = "Dance-Man",
                    Adult = true,
                    Overview = "Second Tester",
                    Budget = 1210000,
                    Genres = new List<Genre>() { Genre.Family, Genre.Mystery },
                    HomePage = "HomePage2",
                    PosterPath = "PosertPage2",
                    ProductionCompanies = new List<ProductionCompany>() { },
                    ProductionCountries = new List<string?>() { "EN" },
                    ReleaseDate = new DateTime(),
                    Revenue = 22000000,
                    Runtime = new TimeSpan(),
                    SpokenLanguages = new List<string?>() { "en" },
                    Status = MovieStatus.Released,
                    VoteAverage = 3.10,
                    VoteCount = 200,
                    Popularity = 4.23,
                    Cast = new List<CastMember>() { },
                    Crew = new List<CrewMember>() { },
                    Reviews = new List<Review>() { },
                    Keywords = new List<Keyword>(),
                }
            );

            context.Add(
                new Movie
                {
                    Id = 2,
                    ImdbId = "movie3",
                    BelongsToCollection = null,
                    Title = "The Third!",
                    Tagline = "Always three of a kind-",
                    OriginalLanguage = "dutch",
                    OriginalTitle = "De Derde",
                    Adult = true,
                    Overview = "Third Tester",
                    Budget = 1202000,
                    Genres = new List<Genre>() { Genre.Mystery },
                    HomePage = "HomePage3",
                    PosterPath = "PosertPage3",
                    ProductionCompanies = new List<ProductionCompany>() { },
                    ProductionCountries = new List<string?>() { "EN" },
                    ReleaseDate = new DateTime(),
                    Revenue = 20003000,
                    Runtime = new TimeSpan(),
                    SpokenLanguages = new List<string?>() { "en" },
                    Status = MovieStatus.Released,
                    VoteAverage = 3.10,
                    VoteCount = 200,
                    Popularity = 4.23,
                    Cast = new List<CastMember>() { },
                    Crew = new List<CrewMember>() { },
                    Reviews = new List<Review>() { },
                    Keywords = new List<Keyword>(),
                }
            );

            context.Add(
                new Movie
                {
                    Id = 3,
                    ImdbId = "movie4",
                    BelongsToCollection = null,
                    Title = "Fourth time's the charm...",
                    Tagline = "Haha thats not the saying!",
                    OriginalLanguage = "english",
                    OriginalTitle = "Fourth time's the charm...",
                    Adult = true,
                    Overview = "Fourth Tester",
                    Budget = 1200400,
                    Genres = new List<Genre>() { Genre.Crime, Genre.Comedy },
                    HomePage = "HomePage4",
                    PosterPath = "PosertPage4",
                    ProductionCompanies = new List<ProductionCompany>() { },
                    ProductionCountries = new List<string?>() { "EN" },
                    ReleaseDate = new DateTime(),
                    Revenue = 25000000,
                    Runtime = new TimeSpan(),
                    SpokenLanguages = new List<string?>() { "en" },
                    Status = MovieStatus.Released,
                    VoteAverage = 3.10,
                    VoteCount = 200,
                    Popularity = 4.23,
                    Cast = new List<CastMember>() { },
                    Crew = new List<CrewMember>() { },
                    Reviews = new List<Review>() { },
                    Keywords = new List<Keyword>(),
                }
            );

            context.Add(
                new Movie
                {
                    Id = 5,
                    ImdbId = "movie5",
                    BelongsToCollection = null,
                    Title = "5 pees in a pod",
                    Tagline = "Already at five!",
                    OriginalLanguage = "italian",
                    OriginalTitle = "piepedie papedie",
                    Adult = true,
                    Overview = "Fith Tester",
                    Budget = 1270000,
                    Genres = new List<Genre>() { Genre.Adventure },
                    HomePage = "HomePage5",
                    PosterPath = "PosertPage5",
                    ProductionCompanies = new List<ProductionCompany>() { },
                    ProductionCountries = new List<string?>() { "US" },
                    ReleaseDate = new DateTime(),
                    Revenue = 20600000,
                    Runtime = new TimeSpan(),
                    SpokenLanguages = new List<string?>() { "en" },
                    Status = MovieStatus.Released,
                    VoteAverage = 3.10,
                    VoteCount = 200,
                    Popularity = 4.23,
                    Cast = new List<CastMember>() { },
                    Crew = new List<CrewMember>() { },
                    Reviews = new List<Review>() { },
                    Keywords = new List<Keyword>(),
                }
            );

            context.Add(
                new Movie
                {
                    Id = 6,
                    ImdbId = "movie6",
                    BelongsToCollection = null,
                    Title = "The Sixth Omen",
                    Tagline = "End in sight",
                    OriginalLanguage = "german",
                    OriginalTitle = "Laatste",
                    Adult = true,
                    Overview = "Sixth Tester",
                    Budget = 1208000,
                    Genres = new List<Genre>() { Genre.Adventure, Genre.Horror },
                    HomePage = "HomePage6",
                    PosterPath = "PosertPage6",
                    ProductionCompanies = new List<ProductionCompany>() { },
                    ProductionCountries = new List<string?>() { "EN" },
                    ReleaseDate = new DateTime(),
                    Revenue = 920000000,
                    Runtime = new TimeSpan(),
                    SpokenLanguages = new List<string?>() { "en" },
                    Status = MovieStatus.Released,
                    VoteAverage = 3.10,
                    VoteCount = 201,
                    Popularity = 4.23,
                    Cast = new List<CastMember>() { },
                    Crew = new List<CrewMember>() { },
                    Reviews = new List<Review>() { },
                    Keywords = new List<Keyword>(),
                }
            );

            return Task.CompletedTask;
        }
    }
}
