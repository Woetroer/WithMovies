namespace WithMovies.Domain.Models
{
    public class Review : BaseEntity
    {
        public virtual User Author { get; set; } = null!;
        public virtual Movie Movie { get; set; } = null!;
        public required double Rating { get; set; }
        public required string? Message { get; set; }
        public required DateTime PostedTime { get; set; }
    }
}
