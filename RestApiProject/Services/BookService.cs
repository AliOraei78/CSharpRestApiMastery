using Microsoft.EntityFrameworkCore;
using RestApiProject.Data;
using RestApiProject.Exceptions;
using RestApiProject.Models;
using Microsoft.Extensions.Caching.Memory;

namespace RestApiProject.Services;

public class BookService : IBookService
{
    private readonly int _serviceId = Random.Shared.Next(1, 100000);
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "AllBooksCacheKey";
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public BookService(AppDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;

        _cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5)) // Removed if not accessed for 5 minutes
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(20)); // Maximum lifetime of 20 minutes
    }

    public async Task<List<Book>> GetAllAsync()
    {
        if (!_cache.TryGetValue(CacheKey, out List<Book> books))
        {
            books = await _context.Books.ToListAsync();

            _cache.Set(CacheKey, books, _cacheOptions);
        }

        return books;
    }


    // GetById remains unchanged
    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books.FindAsync(id);
    }

    // Other methods remain unchanged (Create, Update, Delete must invalidate the cache)

    public async Task<Book> CreateAsync(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        // Invalidate the cache
        _cache.Remove(CacheKey);

        return book;
    }

    public async Task UpdateAsync(int id, Book updatedBook)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) throw new NotFoundException($"Book with id {id} not found");

        book.Title = updatedBook.Title;
        book.Author = updatedBook.Author;
        book.Year = updatedBook.Year;
        book.Price = updatedBook.Price;

        await _context.SaveChangesAsync();

        // Invalidate the cache
        _cache.Remove(CacheKey);
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) throw new NotFoundException($"Book with id {id} not found");

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        // Invalidate the cache
        _cache.Remove(CacheKey);
    }

    public int GetServiceId() => _serviceId;
}