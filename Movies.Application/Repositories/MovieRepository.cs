using Movies.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Application.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly List<Movie> _movies = new List<Movie>();
        public Task<bool> CreateMovieAsync(Movie movie)
        {
            _movies.Add(movie);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteMovieAsync(Guid id)
        {
            var removeCount = _movies.RemoveAll(m => m.Id == id);
            var movieMoved = removeCount > 0;
            return Task.FromResult(movieMoved);
        }

        public Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return Task.FromResult(_movies.AsEnumerable());
        }

        public Task<Movie?> GetMovieByIdAsync(Guid id)
        {
            var movie = _movies.FirstOrDefault(m => m.Id == id);
            return Task.FromResult(movie);
        }

        public Task<bool> UpdateMovieAsync(Movie movie)
        {
            var existingMovie = _movies.FindIndex(m => m.Id == movie.Id);
            if (existingMovie == -1)
            {
                return Task.FromResult(false);
            }
            _movies[existingMovie] = movie;
            return Task.FromResult(true);
        }
    }
}
