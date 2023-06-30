using Microsoft.EntityFrameworkCore;
using WithMovies.Business;
using WithMovies.Business.Services;
using WithMovies.Domain.Interfaces;

namespace WithMovies.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.json");

            builder.Services.AddDbContext<DataContext>(
                options => options.UseSqlite("Data Source=db.sqlite3;")
                                  .UseLazyLoadingProxies());

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IProductionCompanyService, ProductionCompanyService>();
            builder.Services.AddScoped<IMovieCollectionService, MovieCollectionService>();
            builder.Services.AddScoped<IKeywordService, KeywordService>();
            builder.Services.AddScoped<IMovieService, MovieService>();

            builder.Services.AddLogging(x => x.ClearProviders()
                                              .AddConfiguration(builder.Configuration.GetSection("Logging"))
                                              .AddColorConsoleLogger(options =>
                                              {
#if DEBUG
                                                  options.Mask.Add(LogLevel.Debug);
#endif
                                              })
                                              .AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None));

            var app = builder.Build();
            var logger = app.Logger;

            using var scope = app.Services.CreateAsyncScope();

            logger.LogInformation("Applying migrations");
            await scope.ServiceProvider.GetRequiredService<DataContext>().Database.MigrateAsync();

            if (args.Length >= 1 && args[0] == "build-db")
            {
                var json = File.Open(Path.Join(Directory.GetCurrentDirectory(), "../dataset/movies.json"), FileMode.Open);

                logger.LogInformation("Parsing movies");
                await scope.ServiceProvider.GetRequiredService<IMovieService>().ImportJsonAsync(json);

                logger.LogInformation("Saving movies to database");
                await scope.ServiceProvider.GetRequiredService<DataContext>().SaveChangesAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}