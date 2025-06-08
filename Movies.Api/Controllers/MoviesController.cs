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
            return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
        }
        [HttpGet(ApiEndPoints.Movies.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie is null)
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
        [HttpPut(ApiEndPoints.Movies.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request)
        {
            var movie = request.MapToMovie(id);
            var updated = await _movieRepository.UpdateMovieAsync(movie);
            if (!updated)
            {
                return NotFound();
            }
            var response = movie.MapToResponse();
            return Ok(response);
        }

        [HttpDelete(ApiEndPoints.Movies.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _movieRepository.DeleteMovieAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
