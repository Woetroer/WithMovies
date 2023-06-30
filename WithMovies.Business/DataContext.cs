using Microsoft.EntityFrameworkCore;
using WithMovies.Domain.Enums;
using WithMovies.Domain.Models;

namespace WithMovies.Business
{
    public class DataContext : DbContext
    {
        public DbSet<CastMember> CastMembers { get; set; } = null!;
        public DbSet<Credits> Credits { get; set; } = null!;
        public DbSet<CrewMember> CrewMembers { get; set; } = null!;
        public DbSet<Keyword> Keywords { get; set; } = null!;
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<MovieCollection> MovieCollections { get; set; } = null!;
        public DbSet<ProductionCompany> ProductionCompanies { get; set; } = null!;
        public DbSet<RecommendationProfile> RecommendationProfiles { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;

        public DataContext() : base() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Movie>()
                .Property(e => e.Genres)
                .HasConversion(
                    v => string.Join(",", v.Select(e => e.ToString("D")).ToArray()),
                    v => v.Split(new[] { ',' })
                      .Select(e => Enum.Parse(typeof(Genre), e))
                      .Cast<Genre>()
                      .ToList()
                );
            modelBuilder
                .Entity<Movie>()
                .Property(e => e.ProductionCountries)
                .HasConversion(
                    v => string.Join(",", v.ToArray()),
                    v => v.Split(new[] { ',' })
                );
            modelBuilder
                .Entity<Movie>()
                .Property(e => e.SpokenLanguages)
                .HasConversion(
                    v => string.Join(",", v.ToArray()),
                    v => v.Split(new[] { ',' })
                );
        }
    }
}