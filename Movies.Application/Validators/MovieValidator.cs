using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Sevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Validators
{
    public class MovieValidator : AbstractValidator<Movie>
    {
        private readonly IMovieRepository _movieRepostiory;
        public MovieValidator(IMovieRepository movieRepostiory)
        {
            _movieRepostiory = movieRepostiory;
            RuleFor(x=> x.Id).NotEmpty();
            RuleFor(x=> x.Title).NotEmpty();
            RuleFor(x=> x.Genres).NotEmpty();
            RuleFor(x=> x.YearOfRelease).LessThanOrEqualTo(DateTime.UtcNow.Year);
            RuleFor(x => x.Slug).MustAsync(ValidateSlug).WithMessage("This movie already in the system");
        }
        private async Task<bool> ValidateSlug(Movie movie, string slug, CancellationToken token)
        {
            var existingMovie = await _movieRepostiory.GetBySlugAsync(slug);
            if (existingMovie is not null)
            {
                return existingMovie.Id == movie.Id;
            }
            return existingMovie is null;
        }
    }
}
