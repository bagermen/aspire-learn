using Aspire.Common.Dto;
using Aspire.Common.Entities;

namespace Aspire.Common;

public interface IBookRepository
{
    public Task<IEnumerable<Book>> GetAllAsync();
    public Task<Book?> GetByIdAsync(int id);
    public Task AddAsync(BookCreateDto entity);
    public Task UpdateAsync(int id, BookPatchDto entity);
    public Task DeleteAsync(int id);
}
