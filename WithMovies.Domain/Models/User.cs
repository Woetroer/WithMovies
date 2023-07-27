using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WithMovies.Domain.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<User> Friends { get; set; }
        public virtual ICollection<Movie> Watchlist { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }

        public int RecommendationProfileId { get; set; }

        [ForeignKey("RecommendationProfileId")]
        public virtual RecommendationProfile RecommendationProfile { get; set; }
        public DateTime LastLogin { get; set; } = DateTime.Now;
    }
}
