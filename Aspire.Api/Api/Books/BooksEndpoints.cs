using Aspire.Common;
using Aspire.Common.Dto;
using Aspire.Common.Entities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Aspire.Api.Api.Books;

public static class BooksEndpoints
{
    public static IEndpointRouteBuilder MapBooksEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/book", async Task<Ok> (BookCreateDto book, IBookRepository repositoty) =>
        {
            await repositoty.AddAsync(book);

            return TypedResults.Ok();
        })
        .WithName("Add Book")
        .WithOpenApi();

        builder.MapPatch("/book/{id}", async Task<Ok> (int id, BookPatchDto book, IBookRepository repositoty) =>
        {
            await repositoty.UpdateAsync(id, book);

            return TypedResults.Ok();
        })
        .WithName("Update Book")
        .WithOpenApi();

        builder.MapDelete("/book/{id}", async Task<Ok> (int id, IBookRepository repositoty) =>
        {
            await repositoty.DeleteAsync(id);

            return TypedResults.Ok();
        })
        .WithName("Delete Book")
        .WithOpenApi();

        builder.MapGet("/book/{id}", async Task<Ok<BookDto?>> (int id, IBookRepository repositoty) =>
        {
            var result = await repositoty.GetByIdAsync(id) is Book book ? (BookDto)book : null;

            return TypedResults.Ok<BookDto?>(result);
        })
        .WithName("Get Book")
        .WithOpenApi();

        builder.MapGet("/books", async Task<Ok<IEnumerable<BookDto>>> (IBookRepository repositoty) =>
        {
            var books = (await repositoty.GetAllAsync()).Select(b => (BookDto)b);

            return TypedResults.Ok(books);
        })
        .WithName("Get Books")
        .WithOpenApi();

        return builder;
    }
}
