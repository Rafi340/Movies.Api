using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Sevices;
using Movies.Contracts.Responses;

namespace Movies.Api.EndPoints.Ratings
{
    public static class GetUserRatingsEndpoint
    {
        public const string Name = "GetUserRatings";
        public static IEndpointRouteBuilder MapGetUserRatings(this IEndpointRouteBuilder app)
        {
            app.MapGet(ApiEndPoints.Ratings.GetUserRatings,
                async (IRatingService ratingService, HttpContext context, CancellationToken token) =>
                {
                    var userId = context.GetUserId();

                    var ratings = await ratingService.GetRatingsForUserAsync(userId!.Value, token);
                    var ratingsReponse = ratings.MapToResponse();
                    return TypedResults.Ok(ratingsReponse);
                }).WithName(Name)
                .Produces<IEnumerable<MovieRatingResponse>>(StatusCodes.Status200OK)
                .RequireAuthorization();
            return app;
        }
    }
}
