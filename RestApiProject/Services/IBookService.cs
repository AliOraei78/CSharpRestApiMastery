using RestApiProject.Models;

namespace RestApiProject.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<Book> CreateAsync(Book book);
    Task<bool> UpdateAsync(int id, Book updatedBook);
    Task<bool> DeleteAsync(int id);

    int GetServiceId();
}