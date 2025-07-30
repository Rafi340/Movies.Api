using Microsoft.AspNetCore.OutputCaching;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Sevices;
using Movies.Contracts.Requests;

namespace Movies.Api.EndPoints.Movies
{
    public static class UpdateMovieEndPoint
    {
        public const string Name = "UpdateMovie";

        public static IEndpointRouteBuilder MapUpdateMovie(this IEndpointRouteBuilder app)
        {
            app.MapPut(ApiEndPoints.Movies.Update, async (
                Guid id,UpdateMovieRequest request, IMovieService movieService,
                HttpContext context,
                IOutputCacheStore outputCacheStore,
                CancellationToken token)
                =>
            {
                var movie = request.MapToMovie(id);
                var userId = context.GetUserId();
                var updated = await movieService.UpdateAsync(movie, userId, token);
                if (updated == null)
                {
                    return Results.NotFound();
                }
                await outputCacheStore.EvictByTagAsync("movies", token);
                var response = movie.MapToResponse();
                return TypedResults.Ok(response);

            });
            return app;
        }
    }
}
