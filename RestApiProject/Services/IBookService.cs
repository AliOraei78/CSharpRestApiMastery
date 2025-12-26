using RestApiProject.Models;

namespace RestApiProject.Services;

public interface IBookService
{
    Task<List<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<Book> CreateAsync(Book book);
    Task UpdateAsync(int id, Book updatedBook);
    Task DeleteAsync(int id);

    int GetServiceId();
}