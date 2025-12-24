using Microsoft.AspNetCore.Mvc;
using RestApiProject.Models;

namespace RestApiProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private static readonly List<Book> _books = new List<Book>
    {
        new Book(1, "1984", "George Orwell", 1949, 120000m),
        new Book(2, "To Kill a Mockingbird", "Harper Lee", 1960, 150000m),
        new Book(3, "The Great Gatsby", "F. Scott Fitzgerald", 1925, 100000m)
    };

    // GET: api/books
    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAll()
    {
        return Ok(_books);
    }

    // GET: api/books/5
    [HttpGet("{id}")]
    public ActionResult<Book> GetById(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        return book is not null ? Ok(book) : NotFound();
    }

    // POST: api/books
    [HttpPost]
    public ActionResult<Book> Create(Book book)
    {
        book.Id = _books.Max(b => b.Id) + 1;
        _books.Add(book);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // PUT: api/books/5
    [HttpPut("{id}")]
    public IActionResult Update(int id, Book updatedBook)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book is null) return NotFound();

        book.Title = updatedBook.Title;
        book.Author = updatedBook.Author;
        book.Year = updatedBook.Year;
        book.Price = updatedBook.Price;

        return NoContent();
    }

    // DELETE: api/books/5
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book is null) return NotFound();

        _books.Remove(book);
        return NoContent();
    }
}