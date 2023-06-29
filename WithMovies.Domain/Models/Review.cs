namespace WithMovies.Domain.Models
{
    public class Review : BaseEntity
    {
        public required int UserId { get; set; }
        public required int MovieId { get; set; }
        public required int Rating { get; set; }
        public string? Message { get; set; }
        public long TimeStamp { get; set; }
    }
}
