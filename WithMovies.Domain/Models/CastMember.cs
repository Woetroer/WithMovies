namespace WithMovies.Domain.Models
{
    public class CastMember : BaseEntity
    {
        public required int CastId { get; set; }
        public virtual ICollection<Movie> Movies { get; set; } = null!;
        public required string Character { get; set; }
        public required int Gender { get; set; }
        public required string Name { get; set; }
        public required int Order { get; set; }
        public required string? ProfilePath { get; set; }
    }
}
