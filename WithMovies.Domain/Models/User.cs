namespace WithMovies.Domain.Models
{
    public class User : IdentityUser
    {
        public required virtual ICollection<User> Friends { get; set; }
        public required virtual ICollection<Movie> Watchlist { get; set; }
        public required virtual ICollection<Review> Reviews { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }
        public virtual ICollection<User> Friends { get; set; } = null!;
        public virtual ICollection<Movie> Watchlist { get; set; } = null!;
        public virtual ICollection<Review> Reviews { get; set; } = null!;
        public virtual RecommendationProfile RecommendationProfile { get; set; } = null!;
        public DateTime LastLogin { get; set; } = DateTime.Now;
    }
}
