using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WithMovies.Domain.Interfaces;
using WithMovies.Domain.Models;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Identity;



namespace WithMovies.Business.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private DataContext _dataContext;

        public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, DataContext dataContext)
        {
            _userManager = userManager;
            _dataContext = dataContext;
            _roleManager = roleManager;
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

        public async Task<int> GetAllCount()
        {
            var users = await _dataContext.Users.CountAsync();
            return users;
        }

        public async Task<List<User>> MostActiveUsers(int amount)
        {
            var users = await _dataContext.Users.OrderByDescending(n => n.Reviews.Count()).Take(amount).ToListAsync();
            return users;
        }

        public async Task<float> AverageReviewsPerUser()
        {
            float users = await _dataContext.Users.CountAsync();
            float reviews = await _dataContext.Reviews.CountAsync ();

            float average = reviews / users;
            return average;
        }

        public async Task ApplyRole(User user)
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
        }

        public async Task<Boolean> GetUserRole(User user)
        {
            var admin = await _userManager.GetRolesAsync(user);
            return admin.Any(s => s == "Admin");
        }
    }
}
