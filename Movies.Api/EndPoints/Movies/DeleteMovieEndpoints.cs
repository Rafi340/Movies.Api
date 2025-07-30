using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Application.Sevices;

namespace Movies.Api.EndPoints.Movies
{
    public static class DeleteMovieEndpoints
    {
        public const string Name = "DeleteMovie";
        public static IEndpointRouteBuilder MapDeleteMovie(this IEndpointRouteBuilder app)
        {
            app.MapDelete(ApiEndPoints.Movies.Delete, async (
                Guid id, IMovieService movieService,
                HttpContext context,
                IOutputCacheStore outputCacheStore,
                CancellationToken token) =>
            {
                var userId = context.GetUserId();
                var deleted = await movieService.DeleteAsync(id, token);
                if (!deleted)
                {
                    return Results.NotFound();
                }
                await outputCacheStore.EvictByTagAsync("movies", token);
                return TypedResults.Ok();
            }).WithName(Name)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(AuthConstants.AdminUserPolicyName);
            return app;
        }
    }
}
