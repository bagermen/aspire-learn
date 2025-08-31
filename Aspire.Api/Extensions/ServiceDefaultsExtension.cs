using Aspire.Api.Otel;

namespace Aspire.Api.Extensions;

public static class ServiceDefaultsExtension
{
    public static TBuilder AddApiServiceDefaults<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        builder.Services.AddSingleton<ApiMetrics>();
        builder.Services.AddSingleton<ApiSource>();

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddMeter(ApiMetrics.Name);
            })
            .WithTracing(tracing =>
            {
                tracing.AddSource(ApiSource.Name);
            });

        return builder.AddServiceDefaults();
    }
}
