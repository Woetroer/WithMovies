using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WithMovies.Domain.Enums;

namespace WithMovies.Domain.Models
{
    public class CrewMember
    {
        public required int MovieId { get; set; }
        public required string CreditId { get; set; }
        public required Department Department { get; set; }
        public required int Gender { get; set; }
        public required Job Job { get; set; }
        public required string Name { get; set; }
        public string? ProfilePath { get; set; }
    }
}
