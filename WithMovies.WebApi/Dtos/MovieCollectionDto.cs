namespace WithMovies.WebApi.Dtos
{
    public class MovieCollectionDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string? PosterPath { get; set; }
        public required string? BackdropPath { get; set; }
        public required int ItemCount { get; set; }
    }
}
