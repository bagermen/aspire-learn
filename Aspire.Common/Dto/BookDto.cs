using Aspire.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aspire.Common.Dto;

public record BookDto(
    string Title,
    decimal Price,
    string? Description,
    int? Id
)
{
    public static explicit operator BookDto(Book book) =>
        new(
            Id: book.Id,
            Title: book.Title,
            Price: book.Price,
            Description: book.Description
        );
};