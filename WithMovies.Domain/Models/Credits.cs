using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WithMovies.Domain.Models
{
    public class Credits : BaseEntity
    {
        public required virtual Movie Movie {get; set;}
        public required virtual ICollection<CastMember> Cast { get; set;}
        public required virtual ICollection<CrewMember> Crew { get; set;}
    }
}
