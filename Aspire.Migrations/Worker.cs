using Aspire.Data;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Aspire.Migrations;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    private static ActivitySource _activitySource = new("migrations");
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = _activitySource.StartActivity("Migrating database", ActivityKind.Client);

        try
        {
            using var score = serviceProvider.CreateScope();
            var dbcontext = score.ServiceProvider.GetRequiredService<NpgsqlDbContext>();

            await dbcontext.Database.MigrateAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            activity?.AddException(ex);
        }

        hostApplicationLifetime.StopApplication();
    }
}
