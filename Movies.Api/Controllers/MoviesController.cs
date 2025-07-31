using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Application.Sevices;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Controllers
{
    [ApiVersion(1.0)]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IOutputCacheStore _outputCacheStore;
        public MoviesController(IMovieService movieService, IOutputCacheStore outputCacheStore)
        {
            _movieService = movieService;
            _outputCacheStore = outputCacheStore;
        }
        
        [HttpPost(ApiEndPoints.Movies.Create)]
        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        //[ServiceFilter(typeof(ApiKeyAuthFilter))]
        [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationFailureResponse),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateMovieRequest request, CancellationToken token)
        {
            var movie = request.MapToMovie();
            await _movieService.CreateAsync(movie, token);
            await _outputCacheStore.EvictByTagAsync("movies", token);
            return CreatedAtAction(nameof(GetV1), new { idOrSlug = movie.Id }, movie);
        }
        [HttpGet(ApiEndPoints.Movies.Get)]
        [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [OutputCache(PolicyName = "MovieCache")]
        //[ResponseCache(Duration = 30, VaryByHeader = "Accept, Accept-Encoding", Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> GetV1([FromRoute] string idOrSlug,
            [FromServices] LinkGenerator linkGenerator,
            CancellationToken  token)
        {
            var userId = HttpContext.GetUserId();
            var movie = Guid.TryParse(idOrSlug.ToString(), out var id) ?
                await _movieService.GetByIdAsync(id, userId, token) 
                : await _movieService.GetBySlugAsync(idOrSlug, userId, token);

            if (movie is null)
            {
                return NotFound();
            }
            var reponse = movie.MapToResponse();
            var movieObj = new {id = movie.Id};
          
            return Ok(reponse);
        }

        //[HttpGet(ApiEndPoints.Movies.Get)]
        //public async Task<IActionResult> GetV2([FromRoute] string idOrSlug,
        //    [FromServices] LinkGenerator linkGenerator,
        //    CancellationToken token)
        //{
        //    var userId = HttpContext.GetUserId();
        //    var movie = Guid.TryParse(idOrSlug.ToString(), out var id) ?
        //        await _movieService.GetByIdAsync(id, userId, token)
        //        : await _movieService.GetBySlugAsync(idOrSlug, userId, token);

        //    if (movie is null)
        //    {
        //        return NotFound();
        //    }
        //    var reponse = movie.MapToResponse();
        //    var movieObj = new { id = movie.Id };
        //    reponse.Links.Add(new Link
        //    {
        //        Href = linkGenerator.GetPathByAction(HttpContext, nameof(GetV1), values: new { idOrSlug = movie.Id }),
        //        Rel = "self",
        //        Type = "GET"
        //    });
        //    reponse.Links.Add(new Link
        //    {
        //        Href = linkGenerator.GetPathByAction(HttpContext, nameof(Update), values: new { idOrSlug = movie.Id }),
        //        Rel = "self",
        //        Type = "PUT"
        //    });
        //    reponse.Links.Add(new Link
        //    {
        //        Href = linkGenerator.GetPathByAction(HttpContext, nameof(Delete), values: new { idOrSlug = movie.Id }),
        //        Rel = "self",
        //        Type = "DELETE"
        //    });
        //    return Ok(reponse);
        //}



        [EnableRateLimiting("sliding")]
        [HttpGet(ApiEndPoints.Movies.GetAll)]
        [OutputCache(PolicyName ="MovieCache")]
        //[ResponseCache(Duration = 30, VaryByQueryKeys =new[] { "title", "yearOfRelease", "sortBy", "page", "pageSize" }, VaryByHeader = "Accept, Accept-Encoding", Location = ResponseCacheLocation.Any)]
        //[Authorize(AuthConstants.TrustedMemberPolicyName)]
        [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync(
            [FromQuery] GetAllMoviesRequest request,
            CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var options = request.MapToOptions()
                .WithUser(userId);
            var movies = await _movieService.GetAllAsync(options,token);
            var count = await _movieService.GetCountAsync(options.Title, options.YearOfRelease, token);
            var moviesResposne = movies.MapToResponse(request.Page, request.PageSize, count);
            return Ok(moviesResposne);
        }
        [HttpPut(ApiEndPoints.Movies.Update)]
        [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationFailureResponse),StatusCodes.Status400BadRequest)]
        [Authorize(AuthConstants.TrustedMemberPolicyName)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMovieRequest request, CancellationToken token)
        {
            var movie = request.MapToMovie(id);
            var userId = HttpContext.GetUserId();
            var updated = await _movieService.UpdateAsync(movie,userId, token);
            if (updated == null)
            {
                return NotFound();
            }
            await _outputCacheStore.EvictByTagAsync("movies", token);
            var response = movie.MapToResponse();
            return Ok(response);
        }

        [HttpDelete(ApiEndPoints.Movies.Delete)]
        [Authorize(AuthConstants.AdminUserPolicyName)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var deleted = await _movieService.DeleteAsync(id, token);
            if (!deleted)
            {
                return NotFound();
            }
            await _outputCacheStore.EvictByTagAsync("movies", token);
            return Ok();
        }
    }
}
