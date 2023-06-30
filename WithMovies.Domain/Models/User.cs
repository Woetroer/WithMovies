using Microsoft.AspNetCore.Identity;

namespace WithMovies.Domain.Models
{
    public class User : IdentityUser
    {
        public required virtual ICollection<User> Friends { get; set; }
        public required virtual ICollection<Movie> Watchlist { get; set; }
        public required virtual ICollection<Review> Reviews { get; set; }
    }
}
