using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Sdk
{
    [Headers("Authorization: Bearer")]
    public interface IMoviesApi
    {
        [Get(ApiEndPoints.Movies.Get)]
        Task<MovieResponse> GetMovieAsync(string idOrSlug);

        [Get(ApiEndPoints.Movies.GetAll)]
        Task<MoviesResponse> GetMoviesAsync(GetAllMoviesRequest request);

        [Post(ApiEndPoints.Movies.Create)]
        Task<MovieResponse> CreateMovieAsync(CreateMovieRequest request);

        [Put(ApiEndPoints.Movies.Update)]
        Task<MovieResponse> UpdateMovieAsync(Guid id, UpdateMovieRequest request);

        [Delete(ApiEndPoints.Movies.Delete)]
        Task DeleteMovieAsync(Guid id);

        [Put(ApiEndPoints.Movies.Rate)]
        Task RateMovieAsync(Guid id, RateMovieRequest request);

        [Delete(ApiEndPoints.Movies.DeleteRating)]
        Task DeleteRatingAsync(Guid id);

        [Get(ApiEndPoints.Ratings.GetUserRatings)]
        Task<IEnumerable<MovieRatingResponse>> GetUserRatingsAsync(CancellationToken token = default);
    }
}
