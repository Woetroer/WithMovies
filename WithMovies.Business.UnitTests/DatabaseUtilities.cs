using Iso639;
using J2N;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Policy;
using WithMovies.Domain.Enums;
using WithMovies.Domain.Models;

namespace WithMovies.Business.UnitTests
{
    public class DatabaseUtilities
    {
        public User TestUser { get; set; }
        public User TestAdmin { get; set; }

        public Movie TestMovie1 { get; set; }
        public Movie TestMovie2 { get; set; }
        public Movie TestMovie3 { get; set; }
        public Movie TestMovie4 { get; set; }
        public Movie TestMovie5 { get; set; }
        public Movie TestMovie6 { get; set; }
        public MovieCollection TestMovieCollection1 { get; set; }
        public MovieCollection TestMovieCollection2 { get; set; }

        public Review TestReview1 { get; set; }
        public Review TestReview2 { get; set; }
        public Review TestReview3 { get; set; }

        public CastMember TestCastmember1 { get; set; }
        public CrewMember TestCrewmember1 { get; set; }
        public ProductionCompany TestCompany1 { get; set; }

        public Keyword TestKeyword1 { get; set; }
        public RecommendationProfile TestRecommendation1 { get; set; }

        private DataContext _dataContext { get; set; }

        public DatabaseUtilities()
        {
            _dataContext = new DataContext(
                new DbContextOptionsBuilder<DataContext>()
                    .UseSqlite("Data Source=test-db.sqlite")
                    .UseLazyLoadingProxies()
                    .Options
            );

            SetValues();
        }

        private void SetValues()
        {
            TestUser = new User
            {
                Email = "username@email.nl",
                UserName = "Test",
                Id = "my cool GUID",
                Friends = new List<User>(),
                Watchlist = new List<Movie>(),
                Reviews = new List<Review>()
            };

            TestMovie1 = new Movie
            {
                Id = 0,
                ImdbId = "movie1",
                BelongsToCollection = TestMovieCollection1,
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
                ProductionCompanies = new List<ProductionCompany>() { TestCompany1 },
                ProductionCountries = new List<string>() { "Atlantis", "Babylon" },
                ReleaseDate = new DateTime(),
                Revenue = 20000000,
                Runtime = new TimeSpan(),
                SpokenLanguages = new List<string>() { "english", "italian" },
                Status = MovieStatus.Released,
                VoteAverage = 3.10,
                VoteCount = 200,
                Popularity = 4.23,
                Cast = new List<CastMember>() { TestCastmember1 },
                Crew = new List<CrewMember>() { TestCrewmember1 },
                Reviews = new List<Review>() { TestReview1, TestReview2, TestReview3 }
            };

            TestMovie2 = new Movie
            {
                Id = 1,
                ImdbId = "movie2",
                BelongsToCollection = TestMovieCollection1,
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
                ProductionCompanies = new List<ProductionCompany>() { TestCompany1 },
                ProductionCountries = new List<string>() { "Atlantis", "India" },
                ReleaseDate = new DateTime(),
                Revenue = 22000000,
                Runtime = new TimeSpan(),
                SpokenLanguages = new List<string>() { "english", "dutch" },
                Status = MovieStatus.Released,
                VoteAverage = 3.10,
                VoteCount = 200,
                Popularity = 4.23,
                Cast = new List<CastMember>() { TestCastmember1 },
                Crew = new List<CrewMember>() { TestCrewmember1 },
                Reviews = new List<Review>() { TestReview1, TestReview2, TestReview3 }
            };

            TestMovie3 = new Movie
            {
                Id = 2,
                ImdbId = "movie3",
                BelongsToCollection = TestMovieCollection1,
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
                ProductionCompanies = new List<ProductionCompany>() { TestCompany1 },
                ProductionCountries = new List<string>() { "Netherlands", "Babylon" },
                ReleaseDate = new DateTime(),
                Revenue = 20003000,
                Runtime = new TimeSpan(),
                SpokenLanguages = new List<string>() { "dutch", "english" },
                Status = MovieStatus.Released,
                VoteAverage = 3.10,
                VoteCount = 200,
                Popularity = 4.23,
                Cast = new List<CastMember>() { TestCastmember1 },
                Crew = new List<CrewMember>() { TestCrewmember1 },
                Reviews = new List<Review>() { TestReview1, TestReview2, TestReview3 }
            };

            TestMovie4 = new Movie
            {
                Id = 3,
                ImdbId = "movie4",
                BelongsToCollection = TestMovieCollection1,
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
                ProductionCompanies = new List<ProductionCompany>() { TestCompany1 },
                ProductionCountries = new List<string>() { "Atlantis", "Babylon" },
                ReleaseDate = new DateTime(),
                Revenue = 25000000,
                Runtime = new TimeSpan(),
                SpokenLanguages = new List<string>() { "english", "italian" },
                Status = MovieStatus.Released,
                VoteAverage = 3.10,
                VoteCount = 200,
                Popularity = 4.23,
                Cast = new List<CastMember>() { TestCastmember1 },
                Crew = new List<CrewMember>() { TestCrewmember1 },
                Reviews = new List<Review>() { TestReview1, TestReview2, TestReview3 }
            };

            TestMovie5 = new Movie
            {
                Id = 5,
                ImdbId = "movie5",
                BelongsToCollection = TestMovieCollection2,
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
                ProductionCompanies = new List<ProductionCompany>() { TestCompany1 },
                ProductionCountries = new List<string>() { "Germany", "Belgium" },
                ReleaseDate = new DateTime(),
                Revenue = 20600000,
                Runtime = new TimeSpan(),
                SpokenLanguages = new List<string>() { "english", "german" },
                Status = MovieStatus.Released,
                VoteAverage = 3.10,
                VoteCount = 200,
                Popularity = 4.23,
                Cast = new List<CastMember>() { TestCastmember1 },
                Crew = new List<CrewMember>() { TestCrewmember1 },
                Reviews = new List<Review>() { TestReview1, TestReview2, TestReview3 }
            };

            TestMovie6 = new Movie
            {
                Id = 6,
                ImdbId = "movie6",
                BelongsToCollection = TestMovieCollection2,
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
                ProductionCompanies = new List<ProductionCompany>() { TestCompany1 },
                ProductionCountries = new List<string>() { "Atlantis", "Babylon" },
                ReleaseDate = new DateTime(),
                Revenue = 920000000,
                Runtime = new TimeSpan(),
                SpokenLanguages = new List<string>() { "english", "italian" },
                Status = MovieStatus.Released,
                VoteAverage = 3.10,
                VoteCount = 200,
                Popularity = 4.23,
                Cast = new List<CastMember>() { TestCastmember1 },
                Crew = new List<CrewMember>() { TestCrewmember1 },
                Reviews = new List<Review>() { TestReview1, TestReview2, TestReview3 }
            };

            TestMovieCollection1 = new MovieCollection
            {
                BackdropPath = "Backdrop1",
                Name = "Collection1:OG's",
                Id = 1,
                PosterPath = "PosterPath11",
                Movies = new List<Movie> { TestMovie1, TestMovie2, TestMovie3, TestMovie4 }
            };

            TestMovieCollection2 = new MovieCollection
            {
                BackdropPath = "Backdrop2",
                Name = "Collection2:ElectricBoogaluu",
                Id = 2,
                PosterPath = "PosterPath12",
                Movies = new List<Movie> { TestMovie5, TestMovie6 }
            };

            TestReview1 = new Review
            {
                Id = 0,
                Author = TestUser,
                Movie = TestMovie1,
                Rating = 5,
                Message = "Best review!",
                PostedTime = new DateTime()
            };

            TestReview2 = new Review
            {
                Id = 1,
                Author = TestUser,
                Movie = TestMovie2,
                Rating = 3,
                Message = "Pretty mid",
                PostedTime = new DateTime()
            };

            TestReview3 = new Review
            {
                Id = 3,
                Author = TestUser,
                Movie = TestMovie3,
                Rating = 1,
                Message = "Awful!",
                PostedTime = new DateTime()
            };

            TestCastmember1 = new CastMember
            {
                Id = 0,
                CastId = 0,
                Movies = new List<Movie> { TestMovie1, TestMovie2, TestMovie3, TestMovie4 },
                Character = "Character1",
                Gender = 0,
                Name = "John Smith",
                Order = 0,
                ProfilePath = "ProfilePath1"
            };

            TestCrewmember1 = new CrewMember
            {
                Id = 0,
                Movies = new List<Movie> { TestMovie1, TestMovie2, TestMovie3, TestMovie4 },
                Department = Department.Art,
                Gender = 1,
                Job = "Animation",
                Name = "Johny Smith",
                ProfilePath = "ProfilePath2"
            };

            TestCompany1 = new ProductionCompany { Id = 0, Name = "TestCompany" };

            TestKeyword1 = new Keyword
            {
                Id = 0,
                Name = "TestKeyword",
                Movies = new List<Movie>() { TestMovie1, TestMovie2, TestMovie5, TestMovie6 }
            };

            TestRecommendation1 = new RecommendationProfile
            {
                Id = 0,
                ExplicitelyLikedGenres = new bool[20],
                GenreWeights = new float[20],
            };
        }

        public async Task Prepare()
        {
            await _dataContext.Database.EnsureDeletedAsync();
            await _dataContext.Database.EnsureCreatedAsync();

            SetValues();

            await _dataContext.AddAsync(TestUser);
            await _dataContext.AddAsync(TestAdmin);

            await _dataContext.AddAsync(TestMovie1);
            await _dataContext.AddAsync(TestMovie2);
            await _dataContext.AddAsync(TestMovie3);
            await _dataContext.AddAsync(TestMovie4);
            await _dataContext.AddAsync(TestMovie5);
            await _dataContext.AddAsync(TestMovie6);
            await _dataContext.AddAsync(TestMovieCollection1);
            await _dataContext.AddAsync(TestMovieCollection2);

            await _dataContext.AddAsync(TestReview1);
            await _dataContext.AddAsync(TestReview2);
            await _dataContext.AddAsync(TestReview3);

            await _dataContext.AddAsync(TestCastmember1);
            await _dataContext.AddAsync(TestCrewmember1);
            await _dataContext.AddAsync(TestCompany1);

            await _dataContext.AddAsync(TestKeyword1);
            await _dataContext.AddAsync(TestRecommendation1);
        }
    }
}
