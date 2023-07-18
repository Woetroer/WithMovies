using WithMovies.Domain.Enums;

namespace WithMovies.Domain.Models
{
    public class CrewMember : BaseEntity
    {
        public virtual ICollection<Movie> Movies { get; set; } = null!;
        public required Department Department { get; set; }
        public required int Gender { get; set; }
        public required string Job { get; set; }
        public required string Name { get; set; }
        public string? ProfilePath { get; set; }
    }
}
