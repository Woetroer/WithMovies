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
            user.IsBlocked = !user.IsBlocked;
        }

        public async Task Delete(User user)
        {
            _dataContext.Users.Remove(user);
            await _dataContext.SaveChangesAsync();
        }

        public async Task ReviewRights(User user)
        {
            user.CanReview = !user.CanReview;
        }

        public Task SetPreferencesAsync(bool[] preferences, User user)
        {
            user.RecommendationProfile.ExplicitelyLikedGenres = preferences;

            _dataContext.Update(user.RecommendationProfile);

            return Task.CompletedTask;
        }

        public User? GetByName(string name)
        {
            var user = _dataContext.Users.Where(n => n.UserName == name).First();
            return user;
        }
    }
}
