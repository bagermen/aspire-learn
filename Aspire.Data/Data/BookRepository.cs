using Aspire.Common;
using Aspire.Common.Dto;
using Aspire.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aspire.Data.Data;

internal class BookRepository(NpgsqlDbContext context) : IBookRepository
{
    public async Task AddAsync(BookCreateDto entity)
    {
        await context.AddAsync((Book)entity);
        await context.SaveChangesAsync();
    }
    public async Task UpdateAsync(int id, BookPatchDto entityUpdate)
    {
        var book = await context.Books.FindAsync(id);
        if (book is null)
        {
            return;
        }

        if (entityUpdate?.Title is not null)
        {
            book.Title = entityUpdate.Title;
        }

        if (entityUpdate?.Price is not null)
        {
            book.Price = (decimal)entityUpdate.Price;
        }

        if (entityUpdate?.Description is not null)
        {
            book.Description = entityUpdate.Description;
        }

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var book = await context.Books.FindAsync(id);

        if (book is not null)
        {
            context.Books.Remove(book);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
        => await context.Books.AsNoTracking().ToListAsync();

    public async Task<Book?> GetByIdAsync(int id)
        => await context.Books.SingleOrDefaultAsync(b => b.Id == id);
}
