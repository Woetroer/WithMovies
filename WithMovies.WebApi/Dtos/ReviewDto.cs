namespace WithMovies.WebApi.Dtos
{
    public class ReviewDto
    {
        public string WatcherTag { get; set; }
        public int MovieId { get; set; }   
        public int Rating { get; set; }
        public string? Message { get; set; }
        public DateTime PostedTime { get; set; }
    }
}
