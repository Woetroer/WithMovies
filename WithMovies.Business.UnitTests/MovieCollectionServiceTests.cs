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
    public class MovieCollectionServiceTests : UnitTestBase<IMovieCollectionService>
    {
        [Theory]
        [InlineData("Collection1:OG's")]
        [InlineData("Collection2:ElectricBoogaluu")]
        public async Task MovieCollectionExists(string name)
        {
            var result = await _service.MovieCollectionExistsAsync(name);
            Assert.True(result);
        }

        [Theory]
        [InlineData("Collection1:OG's", 1)]
        [InlineData("Collection2:ElectricBoogaluu", 2)]
        public async Task MovieCollectionGetByName(string name, int expected)
        {
            var result = await _service.MovieCollectionGetByNameAsync(name);

            Assert.Equal(result?.Id, expected);
        }

        [Theory]
        [InlineData("Coolest collection", "PosterIshere", "AndHereBackdrop", null)]
        public async Task MovieCollectionCreate(string name, string posterPath, string backdropPath, Movie[] movies)
        {
            var collection = await _service.MovieCollectionCreateAsync(name, posterPath, backdropPath, movies);

            var result = await _service.MovieCollectionGetByNameAsync(collection.Name);

            Assert.Equal(collection?.PosterPath, result?.PosterPath);
        }
        protected override Task SetupDatabase(DataContext context)
        {

            var testMovieCollection1 = new MovieCollection
            {
                BackdropPath = "Backdrop1",
                Name = "Collection1:OG's",
                Id = 1,
                PosterPath = "PosterPath11",
                Movies = new List<Movie> { }
            };

            var testMovieCollection2 = new MovieCollection
            {
                BackdropPath = "Backdrop2",
                Name = "Collection2:ElectricBoogaluu",
                Id = 2,
                PosterPath = "PosterPath12",
                Movies = new List<Movie> { }
            };

            context.Add(testMovieCollection1);
            context.Add(testMovieCollection2);

            return Task.CompletedTask;
        }
    }
}
