using Microsoft.EntityFrameworkCore;
using RestApiProject.Data;
using RestApiProject.Models;

namespace RestApiProject.Services;

public class BookService : IBookService
{
    private readonly int _serviceId = Random.Shared.Next(1, 100000);
    private readonly AppDbContext _context;

    public BookService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Book>> GetAllAsync() => await _context.Books.ToListAsync();

    public async Task<Book?> GetByIdAsync(int id) => await _context.Books.FindAsync(id);

    public async Task<Book> CreateAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }
    public async Task UpdateAsync(int id, Book updatedBook)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) throw new KeyNotFoundException("Book not found");

        book.Title = updatedBook.Title;
        book.Author = updatedBook.Author;
        book.Year = updatedBook.Year;
        book.Price = updatedBook.Price;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) throw new KeyNotFoundException("Book not found");

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }

    public int GetServiceId() => _serviceId;
}