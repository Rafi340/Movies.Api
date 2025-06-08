using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers
{

    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        [HttpPost(ApiEndPoints.Movies.Create)]
        public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
        {
            var movie = request.MapToMovie();
            var created = await _movieRepository.CreateMovieAsync(movie);
            return Created($"/{ApiEndPoints.Movies.Create}/{movie.Id}",movie);
        }
        [HttpGet(ApiEndPoints.Movies.Get)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            if(movie is null)
            {
                return NotFound();
            }
            return Ok(movie.MapToResponse());
        }

        [HttpGet(ApiEndPoints.Movies.GetAll)]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _movieRepository.GetAllMoviesAsync();
            return Ok(movies.Select(m => m.MapToResponse()));
        }
    }
}
