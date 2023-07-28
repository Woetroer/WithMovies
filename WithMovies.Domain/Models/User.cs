using Microsoft.AspNetCore.Identity;

namespace WithMovies.Domain.Models
{
    public class User : IdentityUser
    {
        public virtual required ICollection<User> Friends { get; set; }
        public virtual required ICollection<Movie> Watchlist { get; set; }
        public virtual required ICollection<Review> Reviews { get; set; }
        public required string? RefreshToken { get; set; }
        public required DateTime RefreshTokenExpiry { get; set; }
        public virtual required RecommendationProfile RecommendationProfile { get; set; }
        public DateTime LastLogin { get; set; } = DateTime.Now;

        public required bool IsBlocked { get; set; }
        public required bool CanReview { get; set; }
    }
}
