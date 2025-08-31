using System.Diagnostics.Metrics;

namespace Aspire.Api.Otel;

public sealed class ApiMetrics
{
    public static string Name = typeof(ApiMetrics).Assembly.GetName()?.Name?.ToLower() ?? "api";

    public Counter<int> WeatherRequest { get; init; }

    public ApiMetrics(IMeterFactory factory)
    {
        var meter = factory.Create(Name);
        WeatherRequest = meter.CreateCounter<int>("api_weather_request");
    }
}
