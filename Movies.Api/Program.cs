using Microsoft.AspNetCore.RateLimiting;
using Movies.Api.Mapping;
using Movies.Application;
using Movies.Application.Database;
using System.Threading.RateLimiting;
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddADatabase(config["Database:ConnectionString"]!);

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.OnRejected = async (context, token) =>
    {
        Console.WriteLine("?? Request rejected: Too Many Requests");
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", token);
    };
    rateLimiterOptions.AddSlidingWindowLimiter("sliding", options =>
        {
            options.PermitLimit = 10;
            options.Window = TimeSpan.FromSeconds(10);
            options.SegmentsPerWindow = 2;
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.QueueLimit = 0;
        });
   
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}
app.UseRouting();
app.UseRateLimiter();
app.UseHttpsRedirection();


app.UseAuthorization();
app.UseMiddleware<ValidationMappingMiddleware>();
app.MapControllers();

var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
app.UseEndpoints( endpoints =>
{
    endpoints.MapControllers(); // Must be inside UseEndpoints
});

await dbInitializer.InitializeAsync();

app.Run();
