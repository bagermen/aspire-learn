using Aspire.Api.Api.Books;
using Aspire.Api.Api.Forecast;
using Aspire.Api.Extensions;
using Aspire.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddDbContext();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapForecastEndpoints();

app.MapGroup("/store")
    .MapBooksEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

app.Run();


