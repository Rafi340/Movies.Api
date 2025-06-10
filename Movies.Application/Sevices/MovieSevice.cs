using FluentValidation;
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
        private readonly IValidator<Movie> _movieValidator;
        public MovieSevice(IMovieRepository movieRepository, IValidator<Movie> movieValidator)
        {
            _movieRepository = movieRepository;
            _movieValidator = movieValidator;
        }
        public async Task<bool> CreateAsync(Movie movie)
        {
            await _movieValidator.ValidateAndThrowAsync(movie);
           return await _movieRepository.CreateAsync(movie);
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

            await _movieValidator.ValidateAndThrowAsync(movie);
            var movieExist = await _movieRepository.ExistByIdAsync(movie.Id);
            if (!movieExist)
            {
                return false;
            }
            return await _movieRepository.UpdateAsync(movie);
        }
    }
}
