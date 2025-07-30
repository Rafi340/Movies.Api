using Movies.Api.EndPoints.Movies;

namespace Movies.Api.EndPoints.Ratings
{
    public static class RatingEndPointExtensions
    {
        public static IEndpointRouteBuilder MapRatingEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapRateMovie();
            app.MapDeleteMovie();
            app.MapGetUserRatings();
            //app.MapRatingEndPoints();
            return app;
        }
    }
}
