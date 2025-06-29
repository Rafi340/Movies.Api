using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Application.Sevices;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers
{
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [Authorize]
        [HttpPut(ApiEndPoints.Movies.Rate)]
        public async Task<IActionResult> RateMovie([FromRoute] Guid id,
            [FromBody] RateMovieRequest request, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var result = await _ratingService.RateMovieAsync(id, request.Rating, userId!.Value, token);
            return result ? Ok() : NotFound();
        }
        [Authorize]
        [HttpDelete(ApiEndPoints.Movies.DeleteRating)]
        public async Task<IActionResult> DeleteRating([FromRoute] Guid id, CancellationToken token)
        {
            var userId = HttpContext.GetUserId();
            var result = await _ratingService.DeleteRatingAsync(id, userId!.Value, token);
            return result ? Ok() : NotFound();
        }

    }
}
