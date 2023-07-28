using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WithMovies.Domain.Models;

namespace WithMovies.Domain.Interfaces
{
    public interface IUserService
    {
        Task Delete(User user);
        Task Block(User user);
        Task ReviewRights(User user);
        Task SetPreferencesAsync(bool[] preferences, User user);
        User? GetByName(string name);
    }
}
