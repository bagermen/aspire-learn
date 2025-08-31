using Aspire.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Aspire.Data.Config;

internal class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(x => x.Title).HasMaxLength(200);
        builder.HasIndex(x => x.Price);
        builder.HasIndex(x => x.Title).IsUnique();
    }
}
