using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;

namespace WithMovies.Business.Services
{
    public class UserService : IUserService
    {

        private DataContext _dataContext;
        public UserService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task Block(User user)
        {
            if (user.IsBlocked = false)
                user.IsBlocked = true;
            if (user.IsBlocked)
                user.IsBlocked = false;

            await _dataContext.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            _dataContext.Users.Remove(user);
            await _dataContext.SaveChangesAsync();

        }

        public async Task ReviewRights(User user)
        {
            if (user.CanReview = false)
                user.CanReview = true;
            if (user.CanReview)
                user.CanReview = false;

            await _dataContext.SaveChangesAsync();
        }
    }
}
