﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken token)
        {
            var movie = request.MapToMovie();
            var created = await _movieService.CreateAsync(movie, token);
            return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie);
        }
        [HttpGet(ApiEndPoints.Movies.Get)]
        public async Task<IActionResult> Get([FromRoute] string idOrSlug)
        {
            var movie = Guid.TryParse(idOrSlug.ToString(), out var id) ?
                await _movieService.GetByIdAsync(id) 
                : await _movieService.GetBySlugAsync(idOrSlug);

            if (movie is null)
            {
                return NotFound();
            }
            return Ok(movie.MapToResponse());
        }

        [HttpGet(ApiEndPoints.Movies.GetAll)]
        public async Task<IActionResult> GetAllAsync(CancellationToken token)
        {
            var movies = await _movieService.GetAllAsync(token);
            var moviesResposne = movies.MapToResponse();
            return Ok(moviesResposne);
        }
        [HttpPut(ApiEndPoints.Movies.Update)]
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
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken tokenn)
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
