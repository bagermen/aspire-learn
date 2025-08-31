using Aspire.Common;
using Aspire.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Aspire.Data;

public static class Extensions
{
    public static IHostApplicationBuilder AddDbContext(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<NpgsqlDbContext>(
            Constants.DbConnection,
            null,
            optionsBuilder => {
                optionsBuilder.UseSnakeCaseNamingConvention();
            }
        );

        builder.Services.AddScoped<IBookRepository, BookRepository>();

        return builder;
    }
}
