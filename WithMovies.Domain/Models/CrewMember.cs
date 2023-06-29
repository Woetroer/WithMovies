using WithMovies.Domain.Enums;

namespace WithMovies.Domain.Models
{
    public class CrewMember : BaseEntity
    {
        public required virtual ICollection<Movie> Movies { get; set; }
        public required virtual Credits Credits { get; set; }
        public required Department Department { get; set; }
        public required int Gender { get; set; }
        public required Job Job { get; set; }
        public required string Name { get; set; }
        public string? ProfilePath { get; set; }
    }
}
