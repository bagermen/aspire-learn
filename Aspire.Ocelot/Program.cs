using Aspire.Ocelot;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.ServiceDiscovery;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

ServiceDiscoveryFinderDelegate serviceDiscoveryFinder = (provider, config, route)
    => new AspireServiceProvider(provider, config, route);


// Ocelot Basic setup
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddOcelot(); // single ocelot.json file in read-only mode

builder.Services
    .AddServiceDiscovery()
    .AddSingleton(serviceDiscoveryFinder)
    .AddOcelot(builder.Configuration);

var app = builder.Build();
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

await app.UseOcelot();
await app.RunAsync();