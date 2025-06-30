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
        private readonly IValidator<GetAllMoviesOptions> _optionsValidator;
        private readonly IRatingRepository _ratingRepository;
        public MovieSevice(IMovieRepository movieRepository, 
            IValidator<Movie> movieValidator,
            IRatingRepository ratingRepository,
            IValidator<GetAllMoviesOptions> optionsValidator)
        {
            _movieRepository = movieRepository;
            _movieValidator = movieValidator;
            _ratingRepository = ratingRepository;
            _optionsValidator = optionsValidator;
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

        public async Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default)
        {
            await _optionsValidator.ValidateAndThrowAsync(options, cancellationToken: token);

            return await _movieRepository.GetAllAsync(options, token);
        }

        public Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken token = default)
        {
            return _movieRepository.GetByIdAsync(id, userId, token);
        }

        public Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken token = default)
        {
            return _movieRepository.GetBySlugAsync(slug, userId, token);
        }

        public async Task<Movie?> UpdateAsync(Movie movie, Guid? userId = default, CancellationToken token = default)
        {

            await _movieValidator.ValidateAndThrowAsync(movie,cancellationToken: token);
            var movieExist = await _movieRepository.ExistByIdAsync(movie.Id, token);
            if (!movieExist)
            {
                return null;
            }

            await _movieRepository.UpdateAsync(movie, token);
            if (!userId.HasValue) 
            {
                var rating = await _ratingRepository.GetRatingAsync(movie.Id, token);
                movie.Rating = rating;
                return movie;
            }
            var ratings = await _ratingRepository.GetUserRatingAsync(movie.Id, userId.Value, token);
            movie.Rating = ratings.Rating;
            movie.UserRating = ratings.Userrating;
            return movie;
        }
    }
}
