using Microsoft.EntityFrameworkCore;
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
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}