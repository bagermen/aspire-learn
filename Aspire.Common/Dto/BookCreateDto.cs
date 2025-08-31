using Aspire.Common.Entities;

namespace Aspire.Common.Dto;

public record BookCreateDto(
    string Title,
    decimal Price,
    string? Description
)
{
    public static explicit operator Book(BookCreateDto book) =>
        new()
        {
            Title = book.Title,
            Price = book.Price,
            Description = book.Description
        };
};
