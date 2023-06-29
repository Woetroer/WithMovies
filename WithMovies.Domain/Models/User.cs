namespace WithMovies.Domain.Models
{
    public class User : BaseEntity
    {
        public required List<string> Friends { get; set; }
        public required List<string> Watchlist { get; set; }
        public required List<Review> Reviews { get; set; }

    }
}
