namespace WithMovies.Domain.Models
{
    public class User : BaseEntity
    {
        public required ICollection<User> Friends { get; set; }
        public required ICollection<Movie> Watchlist { get; set; }
        public required ICollection<Review> Reviews { get; set; }

    }
}
