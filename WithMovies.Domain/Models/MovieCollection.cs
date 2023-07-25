namespace WithMovies.Domain.Models
{
    public class MovieCollection : BaseEntity
    {
        public required string Name { get; set; }
        public required string? PosterPath { get; set; }
        public required string? BackdropPath { get; set; }
        public virtual ICollection<Movie> Movies { get; set; } = null!;
    }
}
