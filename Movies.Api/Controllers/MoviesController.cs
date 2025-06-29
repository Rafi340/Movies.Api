using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Sevices;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers
{

    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        
        [HttpPost(ApiEndPoints.Movies.Create)]
        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken token)
        {
            var movie = request.MapToMovie();
            var created = await _movieService.CreateAsync(movie, token);
            return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
        }
        [HttpGet(ApiEndPoints.Movies.Get)]
        public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken  token)
        {
            var userId = HttpContext.GetUserId();
            var movie = Guid.TryParse(idOrSlug.ToString(), out var id) ?
                await _movieService.GetByIdAsync(id, userId, token) 
                : await _movieService.GetBySlugAsync(idOrSlug, userId, token);

            if (movie is null)
            {
                return NotFound();
            }
            return Ok(movie.MapToResponse());
        }

        [EnableRateLimiting("sliding")]
        [HttpGet(ApiEndPoints.Movies.GetAll)]
        public async Task<IActionResult> GetAllAsync(CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var movies = await _movieService.GetAllAsync(userId,token);
            var moviesResposne = movies.MapToResponse();
            return Ok(moviesResposne);
        }
        [HttpPut(ApiEndPoints.Movies.Update)]
        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request, CancellationToken token)
        {
            var movie = request.MapToMovie(id);
            var updated = await _movieService.UpdateAsync(movie, token);
            if (!updated)
            {
                return NotFound();
            }
            var response = movie.MapToResponse();
            return Ok(response);
        }

        [HttpDelete(ApiEndPoints.Movies.Delete)]
        [Authorize(AuthConstants.AdminUserPolicyName)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var deleted = await _movieService.DeleteAsync(id, token);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
