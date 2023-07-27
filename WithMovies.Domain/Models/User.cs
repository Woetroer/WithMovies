using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WithMovies.Domain.Models
{
    public class User : IdentityUser
    {
        public virtual required ICollection<User> Friends { get; set; }
        public virtual required ICollection<Movie> Watchlist { get; set; }
        public virtual required ICollection<Review> Reviews { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }
        public virtual required RecommendationProfile RecommendationProfile { get; set; }
        public DateTime LastLogin { get; set; } = DateTime.Now;
    }
}
