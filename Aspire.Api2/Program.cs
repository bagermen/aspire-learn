using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient("api", client => {
    client.BaseAddress = new("https+http://aspire-api");
}).AddAsKeyed();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}


app.MapGet("/weatherforecast", async Task<Results<Ok<IAsyncEnumerable<WeatherForecast?>>, BadRequest>> ([FromKeyedServices("api")] HttpClient client) =>
{
    try
    {
        var response = await client.GetAsync("/weatherforecast", HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();
        return TypedResults.Ok(response.Content.ReadFromJsonAsAsyncEnumerable<WeatherForecast>());
    }
    catch (HttpRequestException e) when (e is { StatusCode: HttpStatusCode.BadRequest })
    {
        return TypedResults.BadRequest();
    }
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
