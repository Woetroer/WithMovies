﻿using WithMovies.Domain.Models;

namespace WithMovies.Domain.Interfaces
{
    public interface IMovieService
    {
        Task ImportJsonAsync(Stream json);
        Task<Movie?> GetByIdAsync(int id);
        Task<List<Movie>> GetPopularMovies();
    }
}
