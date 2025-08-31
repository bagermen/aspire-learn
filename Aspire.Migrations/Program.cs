using Aspire.Migrations;
using Aspire.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.AddDbContext();
builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services
    .AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource("migrations"));

var host = builder.Build();
host.Run();
