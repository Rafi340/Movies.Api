
using Movies.Api.Sdk;
using Movies.Contracts.Requests;
using Refit;
using System.Text.Json;

var moviesApi = RestService.For<IMoviesApi>("https://localhoast:5003");

var movie = await moviesApi.GetMovieAsync("some-movie-id-or-slug");

var request = new GetAllMoviesRequest { Page = 1, PageSize = 10,
    Title = null,
    YearOfRelease = null,
    SortBy = null
};
var movies = await moviesApi.GetMoviesAsync(request);

foreach (var movieResponse in movies.Items)
{
    Console.WriteLine(JsonSerializer.Serialize(movieResponse));
}


