using Aspire.Common.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Aspire.Data;

public class NpgsqlDbContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public NpgsqlDbContext(DbContextOptions<NpgsqlDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
