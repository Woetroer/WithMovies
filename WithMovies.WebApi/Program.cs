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

            builder.Services.AddDbContext<DataContext>(options => options.UseSqlite("Data Source=db.sqlite3;").UseLazyLoadingProxies());

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IKeywordService, KeywordService>();
            builder.Services.AddScoped<IMovieService, MovieService>();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}