using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WithMovies.Domain;
using WithMovies.Domain.Enums;
using WithMovies.Domain.Models;

namespace WithMovies.Business
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<CastMember> CastMembers { get; set; } = null!;
        public DbSet<CrewMember> CrewMembers { get; set; } = null!;
        public DbSet<Keyword> Keywords { get; set; } = null!;
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<MovieCollection> MovieCollections { get; set; } = null!;
        public DbSet<ProductionCompany> ProductionCompanies { get; set; } = null!;
        public DbSet<RecommendationProfile> RecommendationProfiles { get; set; } = null!;
        public DbSet<RecommendationProfileInput> RecommendationProfileInputs { get; set; } = null!;
        public DbSet<WeightedMovie> WeightedMovies { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;

        // Fake table, doesn't actually exist. This is used in KeywordService
        public virtual DbSet<KeywordRecord> KeywordRecords { get; set; } = null!;

        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        protected DataContext(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Movie>()
                .Property(e => e.Genres)
                .HasConversion(
                    v => string.Join(",", v.Select(e => e.ToString("D")).ToArray()),
                    v =>
                        string.IsNullOrWhiteSpace(v)
                            ? new List<Genre>()
                            : v.Split(new[] { ',' })
                                .Select(e => Enum.Parse(typeof(Genre), e))
                                .Cast<Genre>()
                                .ToList()
                );
            modelBuilder
                .Entity<Movie>()
                .Property(e => e.ProductionCountries)
                .HasConversion(
                    v => string.Join(",", v.ToArray()),
                    v => string.IsNullOrWhiteSpace(v) ? new List<string?>() : v.Split(new[] { ',' })
                );
            modelBuilder
                .Entity<Movie>()
                .Property(e => e.SpokenLanguages)
                .HasConversion(
                    v => string.Join(",", v.ToArray()),
                    v => string.IsNullOrWhiteSpace(v) ? new List<string?>() : v.Split(new[] { ',' })
                );

            modelBuilder
                .Entity<RecommendationProfile>()
                .Property(p => p.ExplicitelyLikedGenres)
                .HasConversion(
                    v => v != null ? v.Select(b => b ? (byte)1 : (byte)0).ToArray() : new byte[20],
                    v => v != null ? v.Select(b => b != 0).ToArray() : new bool[20]
                );

            modelBuilder.Entity<CastMember>().HasMany(m => m.Movies).WithMany(m => m.Cast);
            modelBuilder.Entity<CrewMember>().HasMany(m => m.Movies).WithMany(m => m.Crew);
            modelBuilder.Entity<Keyword>().HasMany(m => m.Movies);
            modelBuilder.Entity<Movie>().HasOne(m => m.BelongsToCollection).WithMany(c => c.Movies);
            modelBuilder
                .Entity<Movie>()
                .HasMany(m => m.ProductionCompanies)
                .WithMany(c => c.Movies);
            modelBuilder.Entity<Movie>().HasMany(m => m.Cast).WithMany(c => c.Movies);
            modelBuilder.Entity<Movie>().HasMany(m => m.Crew).WithMany(c => c.Movies);
            modelBuilder.Entity<Movie>().HasMany(m => m.Reviews).WithOne(c => c.Movie);
            modelBuilder
                .Entity<MovieCollection>()
                .HasMany(m => m.Movies)
                .WithOne(c => c.BelongsToCollection);
            modelBuilder
                .Entity<ProductionCompany>()
                .HasMany(m => m.Movies)
                .WithMany(c => c.ProductionCompanies);
            modelBuilder.Entity<Review>().HasOne(r => r.Author).WithMany(c => c.Reviews);
            modelBuilder.Entity<Review>().HasOne(r => r.Movie).WithMany(c => c.Reviews);
            modelBuilder.Entity<User>().HasMany(m => m.Friends);
            modelBuilder.Entity<User>().HasMany(m => m.Watchlist);
            modelBuilder.Entity<User>().HasMany(m => m.Reviews).WithOne(r => r.Author);

            modelBuilder.Entity<KeywordRecord>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }
    }
}
