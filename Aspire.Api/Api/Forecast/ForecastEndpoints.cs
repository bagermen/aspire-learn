using Aspire.Api.Otel;

namespace Aspire.Api.Api.Forecast;

public static class ForecastEndpoints
{
    public static IEndpointRouteBuilder MapForecastEndpoints(this IEndpointRouteBuilder builder)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        builder.MapGet("/weatherforecast", (ApiMetrics metrics, ApiSource source, ILogger<ApiMetrics> logger, HttpRequest request) =>
        {
            using (source.Activity.StartActivity("api_send_metrics"))
            {
                logger.LogInformation("отсылаем метрику WeatherRequest");
                metrics.WeatherRequest.Add(1);
            }

            using var span = source.Activity.StartActivity("api_send_weather");
            logger.LogInformation("отсылаем погоду");

            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast");

        return builder;
    }
}
