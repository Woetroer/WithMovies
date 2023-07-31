namespace WithMovies.Domain
{
    public class PreviewDto
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string? PosterPath { get; set; }
        public required string Tagline { get; set; }
    }
}
