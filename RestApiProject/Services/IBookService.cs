using RestApiProject.Models;

namespace RestApiProject.Services;

public interface IBookService
{
    IEnumerable<Book> GetAll();
    Book? GetById(int id);
    Book Create(Book book);
    bool Update(int id, Book updatedBook);
    bool Delete(int id);

    int GetServiceId();
}