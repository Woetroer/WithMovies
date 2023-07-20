using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using WithMovies.Business.Services;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Business.UnitTests
{
    public class UnitTestReviewService
    {

        IServiceCollection _services {get; set;}

        public UnitTest1()
        {
            _services = new ServiceCollection();
            _services.AddDbContext<DataContext>(
                options => options.UseSqlite("Data Source=test-db.sqlite3;")
                                  .UseLazyLoadingProxies());
            _services.AddScoped<IProductionCompanyService, ProductionCompanyService>();
            _services.AddScoped<IMovieCollectionService, MovieCollectionService>();
            _services.AddScoped<IKeywordService, KeywordService>();
            _services.AddScoped<IMovieService, MovieService>();
        }

        [Fact]
        public void Test1()
        {
            
        }
    }
}