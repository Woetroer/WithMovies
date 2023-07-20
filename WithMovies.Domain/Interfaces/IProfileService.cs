using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WithMovies.Domain.Interfaces
{
    public interface IProfileService
    {
        Task Delete(int id);
        Task Block(int id);
        Task ReviewRights(int id);
    }
}
