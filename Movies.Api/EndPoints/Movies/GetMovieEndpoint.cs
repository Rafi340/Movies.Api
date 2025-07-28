using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Sevices;

namespace Movies.Api.EndPoints.Movies
{
    public static class GetMovieEndpoint
    {
        public const string Name = "GetMovie";
        public static IEndpointRouteBuilder MapGetMovie(this IEndpointRouteBuilder app)
        {
            app.MapGet(ApiEndPoints.Movies.Get, async (
                string idOrSlug, IMovieService movieService,
                HttpContext context,
                CancellationToken token) =>
            {
                var userId = context.GetUserId();
                var movie = Guid.TryParse(idOrSlug.ToString(), out var id) ?
                    await movieService.GetByIdAsync(id, userId, token)
                    : await movieService.GetBySlugAsync(idOrSlug, userId, token);

                if (movie is null)
                {
                    return Results.NotFound();
                }
                var reponse = movie.MapToResponse();
                var movieObj = new { id = movie.Id };

                return TypedResults.Ok(reponse);
            }).WithName(Name);
            return app;
        }
    }
}
