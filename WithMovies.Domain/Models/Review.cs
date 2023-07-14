namespace WithMovies.Domain.Models
{
    public class Review : BaseEntity
    {
        public required virtual User User { get; set; }
        public required int MovieId { get; set; }
        public required int Rating { get; set; }
        public string? Message { get; set; }
        public required DateTime PostedTime { get; set; }
    }
}
