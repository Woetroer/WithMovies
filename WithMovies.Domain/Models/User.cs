using Microsoft.AspNetCore.Identity;

namespace WithMovies.Domain.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<User> Friends { get; set; } = null!;
        public virtual ICollection<Movie> Watchlist { get; set; } = null!;
        public virtual ICollection<Review> Reviews { get; set; } = null!;
        public virtual RecommendationProfile RecommendationProfile { get; set; } = null!;
        public DateTime LastLogin { get; set; } = DateTime.Now;
    }
}
