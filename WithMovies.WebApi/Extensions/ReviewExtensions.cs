﻿using WithMovies.Domain.Models;
using WithMovies.WebApi.Dtos;

namespace WithMovies.WebApi.Extensions
{
    public static class ReviewExtensions
    {
        public static ReviewDto ToDto(this Review review)
        {
            var dto = new ReviewDto();

            dto.WatcherTag = review.Author.UserName!;
            dto.Rating = review.Rating;
            dto.Message = review.Message;
            dto.MovieId = review.Movie.Id;
            dto.PostedTime = review.PostedTime;

            return dto;
        }
    }
}
