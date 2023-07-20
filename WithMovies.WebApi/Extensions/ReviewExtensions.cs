using WithMovies.Domain.Models;
using WithMovies.WebApi.Dtos;

namespace WithMovies.WebApi.Extensions
{
    public static class ReviewExtensions
    {
        public static ReviewDto ToDto (this Review review)
        {
            ReviewDto dto = new ReviewDto ();
            dto.WatcherTag = review.User.UserName;
            dto.Rating = review.Rating;
            dto.Message = review.Message;
            dto.MovieId = review.MovieId;
            dto.PostedTime = review.PostedTime;

            return dto;
        }
    }
}
