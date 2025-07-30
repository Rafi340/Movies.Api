using Movies.Api.Auth;
using Movies.Application.Sevices;
using Movies.Contracts.Requests;

namespace Movies.Api.EndPoints.Ratings
{
    public static class RateMovieEndpoint
    {
        public const string Name = "RateMovie";
        public static IEndpointRouteBuilder MapRateMovie(this IEndpointRouteBuilder app)
        {
            app.MapPut(ApiEndPoints.Movies.Rate,
                async (Guid id, RateMovieRequest request, IRatingService ratingService,
                    HttpContext context, CancellationToken token) =>
                {
                    var userId = context.GetUserId();
                    var result = await ratingService.RateMovieAsync(id, request.Rating, userId!.Value, token);
                    return result ? TypedResults.Ok() : Results.NotFound();
                }).WithName(Name).RequireAuthorization();
            return app;
        }
    }
}
