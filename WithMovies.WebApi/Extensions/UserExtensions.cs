using System.Runtime.CompilerServices;
using WithMovies.Domain.Models;
using WithMovies.WebApi.Dtos;

namespace WithMovies.WebApi.Extensions
{
    public static class UserExtensions
    {
        public static UserDto ToDto(this User user, string UserName)
        {
            UserDto dto = new UserDto();
            dto.UserName = UserName;
            dto.IsBlocked = user.IsBlocked;
            dto.LastOnline = user.LastLogin.ToLongDateString();
            dto.Reviews = user.Reviews.Count();

            return dto;
        }
    }
}

