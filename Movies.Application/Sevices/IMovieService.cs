﻿using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Sevices
{
    public interface IMovieService
    {
        Task<bool> CreateAsync(Movie movie, CancellationToken token = default);
        Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default);
        Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default);
        Task<IEnumerable<Movie>> GetAllAsync(Guid? userId = default, CancellationToken token = default);
        Task<Movie?> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken token = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken token = default);
        Task<bool> ExistByIdAsync(Guid id, CancellationToken token = default);
    }
}
