namespace WithMovies.Domain.Models
{
    public class Review : BaseEntity
    {
        public required virtual User User { get; set; }
        public required virtual Movie Movie { get; set; }
        public required double Rating { get; set; }
        public required string? Message { get; set; }
        public required DateTime PostedTime { get; set; }
    }
}
