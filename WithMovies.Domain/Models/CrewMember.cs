using WithMovies.Domain.Enums;

namespace WithMovies.Domain.Models
{
    public class CrewMember : BaseEntity
    {
        public required List<int> MovieIds { get; set; }
        public required string CreditId { get; set; }
        public required Department Department { get; set; }
        public required int Gender { get; set; }
        public required Job Job { get; set; }
        public required string Name { get; set; }
        public string? ProfilePath { get; set; }
    }
}
