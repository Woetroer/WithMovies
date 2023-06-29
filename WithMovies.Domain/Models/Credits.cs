using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WithMovies.Domain.Models
{
    public class Credits : BaseEntity
    {
        public required int MovieId {get; set;}
        public required string Cast { get; set;}
        public required string Crew { get; set;}
    }
}
