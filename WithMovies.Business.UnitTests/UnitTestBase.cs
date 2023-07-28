using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WithMovies.Business.Services;
using WithMovies.Data.Sqlite;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Business.UnitTests;

public abstract class UnitTestBase
{
    protected IServiceProvider _services;
    private DataContext? _dataContext;

    public UnitTestBase()
    {
        var collection = new ServiceCollection();

        collection.AddDbContext<DataContext>(
            options =>
                options
                    .UseSqlite(
                        "Data Source=test.db;",
                        x => x.MigrationsAssembly("WithMovies.Business")
                    )
                    .UseLazyLoadingProxies()
        );
        collection.AddScoped<IProductionCompanyService, ProductionCompanyService>();
        collection.AddScoped<IMovieCollectionService, MovieCollectionService>();
        collection.AddScoped<IKeywordService, SqliteKeywordService>();
        collection.AddScoped<IReviewService, ReviewService>();
        collection.AddScoped<IMovieService, SqliteMovieService>();
        collection.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<DataContext>();
        collection.AddLogging();

        _services = collection.BuildServiceProvider();

        Task.WaitAll(TestStart());
    }

    protected abstract Task SetupDatabase(DataContext context);

    protected Task PreDisposeDatabase(DataContext context) => Task.CompletedTask;

    protected Task PostDisposeDatabase() => Task.CompletedTask;

    private async Task TestStart()
    {
        _dataContext = _services.GetRequiredService<DataContext>();
        await _dataContext.Database.EnsureDeletedAsync();
        await _dataContext.Database.EnsureCreatedAsync();

        await SetupDatabase(_dataContext);
        await _dataContext.SaveChangesAsync();
    }

    private void RemoveAll()
    {
        RemoveAll<CastMember>();
        RemoveAll<CrewMember>();
        RemoveAll<Keyword>();
        RemoveAll<Movie>();
        RemoveAll<MovieCollection>();
        RemoveAll<ProductionCompany>();
        RemoveAll<RecommendationProfile>();
        RemoveAll<Review>();
    }

    private void RemoveAll<T>()
        where T : class
    {
        _dataContext!.RemoveRange(_dataContext.Set<T>());
    }
}

public abstract class UnitTestBase<T> : UnitTestBase
    where T : notnull
{
    protected T _service;

    public UnitTestBase()
        : base()
    {
        _service = _services.GetRequiredService<T>();
    }
}
