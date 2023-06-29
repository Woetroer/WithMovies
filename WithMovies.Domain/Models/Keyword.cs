using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WithMovies.Domain.Models
{
    public class Keyword : BaseEntity
    {
        public required virtual ICollection<Movie> Movies { get; set; }
        public required string Name { get; set; }
    }
}
