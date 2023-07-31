using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;
using Microsoft.Data.Sqlite;


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

        public async Task<User?> GetByName(string name)
        {
            var user = await _dataContext.Users.Where(n => n.UserName == name).FirstOrDefaultAsync();
            return user;
        }

        public async Task<List<User>> GetAll()
        {
            var users = await _dataContext.Users.ToListAsync();
            return users;
        }

        public async Task<List<User>> TopTenMostActiveUsers()
        {
            var users = await _dataContext.Users.OrderByDescending(n => n.Reviews.Count).Take(10).ToListAsync();
            return users;
        }

        public async Task<float> AverageActivity()
        {
            float users = _dataContext.Users.Count();
            float reviews = _dataContext.Reviews.Count();

            float average = reviews / users;
            return average;
        }
    }
}
