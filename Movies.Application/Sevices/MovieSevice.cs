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
        public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
        {
            await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken: token);
           return await _movieRepository.CreateAsync(movie, token);
        }

        public Task<bool> DeleteAsync(Guid id , CancellationToken token = default)
        {
            return _movieRepository.DeleteAsync(id , token);
        }

        public Task<bool> ExistByIdAsync(Guid id, CancellationToken token = default)
        {
            return _movieRepository.ExistByIdAsync(id, token);
        }

        public Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default)
        {
            return _movieRepository.GetAllAsync(token);
        }

        public Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
        {
            return _movieRepository.GetByIdAsync(id, token);
        }

        public Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
        {
            return _movieRepository.GetBySlugAsync(slug, token);
        }

        public async Task<bool> UpdateAsync(Movie movie, CancellationToken token = default)
        {

            await _movieValidator.ValidateAndThrowAsync(movie,cancellationToken: token);
            var movieExist = await _movieRepository.ExistByIdAsync(movie.Id, token);
            if (!movieExist)
            {
                return false;
            }
            return await _movieRepository.UpdateAsync(movie, token);
        }
    }
}
