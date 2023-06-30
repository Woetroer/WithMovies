namespace WithMovies.Domain.Models
{
    public class CastMember : BaseEntity
    {
        public required int CastId { get; set; }
        public required virtual Credits Credits { get; set; }
        public required virtual ICollection<Movie> Movies { get; set; }
        public required string Character { get; set; }
        public required int Gender { get; set; }
        public required string Name { get; set; }
        public required int Order { get; set; }
        public required string? ProfilePath { get; set; }
    }
}
