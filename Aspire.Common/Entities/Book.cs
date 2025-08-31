namespace Aspire.Common.Entities;

public class Book : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; } = default;
}
