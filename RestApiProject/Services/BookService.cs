using RestApiProject.Models;

namespace RestApiProject.Services;

public class BookService : IBookService
{
    private readonly int _serviceId = Random.Shared.Next(1, 100000);
    private readonly List<Book> _books = new()
    {
        new Book(1, "1984", "George Orwell", 1949, 120000m),
        new Book(2, "To Kill a Mockingbird", "Harper Lee", 1960, 150000m),
        new Book(3, "The Great Gatsby", "F. Scott Fitzgerald", 1925, 100000m)
    };

    public async Task<IEnumerable<Book>> GetAllAsync() => await Task.FromResult(_books);

    public async Task<Book?> GetByIdAsync(int id) => await Task.FromResult(_books.FirstOrDefault(b => b.Id == id));

    public async Task<Book> CreateAsync(Book book)
    {
        book.Id = _books.Max(b => b.Id) + 1;
        _books.Add(book);
        return await Task.FromResult(book);
    }

    public async Task<bool> UpdateAsync(int id, Book updatedBook)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book is null) return false;

        book.Title = updatedBook.Title;
        book.Author = updatedBook.Author;
        book.Year = updatedBook.Year;
        book.Price = updatedBook.Price;
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book is null) return false;

        _books.Remove(book);
        return true;
    }

    public int GetServiceId() => _serviceId;
}