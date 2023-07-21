using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WithMovies.Domain.Models
{
    public class Keyword : BaseEntity
    {
        public required string Name { get; set; }
        public virtual ICollection<Movie> Movies { get; set; } = null!;
    }
}
