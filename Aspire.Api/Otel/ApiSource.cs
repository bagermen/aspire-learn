using System.Diagnostics;

namespace Aspire.Api.Otel;

public sealed class ApiSource : IDisposable
{
    public static string Name = typeof(ApiSource).Assembly.GetName()?.Name?.ToLower() ?? "api";

    public ActivitySource Activity { get; init; } = new ActivitySource(Name, typeof(ApiSource).Assembly.GetName()?.Version?.ToString() ?? "0.0");
    public void Dispose()
    {
        Activity.Dispose();
    }
}
