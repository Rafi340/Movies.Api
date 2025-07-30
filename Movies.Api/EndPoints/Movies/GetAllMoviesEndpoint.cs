using Microsoft.AspNetCore.Mvc;
using Movies.Api.Auth;
using Movies.Api.Mapping;
using Movies.Application.Sevices;
using Movies.Contracts.Requests;

namespace Movies.Api.EndPoints.Movies
{
    public static class GetAllMoviesEndpoint
    {
        public const string Name = "GetMovies";
        public static IEndpointRouteBuilder MapGetAllMovies(this IEndpointRouteBuilder app)
        {
            app.MapGet(ApiEndPoints.Movies.GetAll, async (
                [AsParameters] GetAllMoviesRequest request,
                IMovieService movieService,
                HttpContext context,
                CancellationToken token) =>
            {
                var userId = context.GetUserId();
                var options = request.MapToOptions()
                    .WithUser(userId);
                var movies = await movieService.GetAllAsync(options, token);
                var count = await movieService.GetCountAsync(options.Title, options.YearOfRelease, token);
                var moviesResposne = movies.MapToResponse(request.Page.GetValueOrDefault(PagedRequest.DefaultPage),
                    request.PageSize.GetValueOrDefault(PagedRequest.DefaultPageSize), count);
                return TypedResults.Ok(moviesResposne);
            }).WithName(Name);
            return app;
        }
    }
}