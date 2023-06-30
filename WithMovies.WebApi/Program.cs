using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WithMovies.Business;
using WithMovies.Business.Services;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

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

            // Authentication
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                    };
                });

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy => { policy.WithOrigins("Hier komt localhost met angular poort").AllowAnyHeader().AllowAnyMethod(); });
            });

            var app = builder.Build();
            var logger = app.Logger;

            using var scope = app.Services.CreateAsyncScope();

            logger.LogInformation("Applying migrations");
            await scope.ServiceProvider.GetRequiredService<DataContext>().Database.MigrateAsync();

            if (args.Length >= 1 && args[0] == "build-db")
            {
                var db = scope.ServiceProvider.GetRequiredService<DataContext>();

                var moviesJson = File.Open(Path.Join(Directory.GetCurrentDirectory(), "../dataset/movies.json"), FileMode.Open);

                logger.LogInformation("Parsing movies");
                await scope.ServiceProvider.GetRequiredService<IMovieService>().ImportJsonAsync(moviesJson);

                logger.LogInformation("Saving movies to database");
                await db.SaveChangesAsync();

                var keywordsJson = File.Open(Path.Join(Directory.GetCurrentDirectory(), "../dataset/keywords.json"), FileMode.Open);

                logger.LogInformation("Parsing keywords");
                await scope.ServiceProvider.GetRequiredService<IKeywordService>().ImportJsonAsync(keywordsJson);

                logger.LogInformation("Saving keywords to database");
                await db.SaveChangesAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
