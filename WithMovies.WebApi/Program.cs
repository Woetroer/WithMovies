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

            builder.Services.AddDbContext<DataContext>(options => options.UseSqlite("Data Source=db.sqlite3;").UseLazyLoadingProxies());

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IKeywordService, KeywordService>();
            builder.Services.AddScoped<IMovieService, MovieService>();

            var app = builder.Build();

            //if (args.Length >= 1 && args[0] == "build-db")
            //{
            //    using var scope = app.Services.CreateAsyncScope();
            //    var json = File.Open(Path.Join(Directory.GetCurrentDirectory(), "../dataset/movies.json"), FileMode.Open);
            //    await scope.ServiceProvider.GetRequiredService<IMovieService>().ImportJsonAsync(json);
            //    await scope.ServiceProvider.GetRequiredService<DataContext>().SaveChangesAsync();
            //}

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