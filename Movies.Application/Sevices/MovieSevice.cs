using Movies.Application.Models;
using Movies.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Sevices
{
    public class MovieSevice : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        public MovieSevice(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        public Task<bool> CreateAsync(Movie movie)
        {
           return _movieRepository.CreateAsync(movie);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            return _movieRepository.DeleteAsync(id);
        }

        public Task<bool> ExistByIdAsync(Guid id)
        {
            return _movieRepository.ExistByIdAsync(id);
        }

        public Task<IEnumerable<Movie>> GetAllAsync()
        {
            return _movieRepository.GetAllAsync();
        }

        public Task<Movie?> GetByIdAsync(Guid id)
        {
            return _movieRepository.GetByIdAsync(id);
        }

        public Task<Movie?> GetBySlugAsync(string slug)
        {
            return _movieRepository.GetBySlugAsync(slug);
        }

        public async Task<bool> UpdateAsync(Movie movie)
        {
            var movieExist = await _movieRepository.ExistByIdAsync(movie.Id);
            if (!movieExist)
            {
                return false;
            }
            return await _movieRepository.UpdateAsync(movie);
        }
    }
}
