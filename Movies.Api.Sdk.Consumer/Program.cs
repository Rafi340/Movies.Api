
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Sdk;
using Movies.Api.Sdk.Consumer;
using Movies.Contracts.Requests;
using Refit;
using System.Text.Json;

//var moviesApi = RestService.For<IMoviesApi>("https://localhoast:5003");

var services = new ServiceCollection();

services
    .AddHttpClient()
    .AddSingleton<AuthTokenProvider>()
    .AddRefitClient<IMoviesApi>(s => new RefitSettings
    {
        AuthorizationHeaderValueGetter = async (request, CancellationToken) => await s.GetRequiredService<AuthTokenProvider>().GetTokenAsync(),
    })
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:5001"));

var provider = services.BuildServiceProvider();

var moviesApi = provider.GetRequiredService<IMoviesApi>();

var movie = await moviesApi.GetMovieAsync("some-movie-id-or-slug");

var newMovie = await moviesApi.CreateMovieAsync(new CreateMovieRequest
{
    Title = "Khan Shabeb the boss",
    YearOfRelease = 2024,
    Genres = new  []{ "Action", "Adventure", "Thriller" }
});

await moviesApi.UpdateMovieAsync(newMovie.Id,new UpdateMovieRequest
{
    Title = "Khan Shabeb the boss",
    YearOfRelease = 2024,
    Genres = new[] { "Action", "Adventure" }
});

await moviesApi.DeleteRatingAsync(newMovie.Id);
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


