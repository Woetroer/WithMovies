using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WithMovies.Domain.Models
{
    public class User : BaseEntity
    {
        public required List<string> Friends { get; set; }
        public required List<string> Watchlist { get; set; }
        
    }
}
