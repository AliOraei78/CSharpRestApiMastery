using Microsoft.AspNetCore.Mvc;
using RestApiProject.Models;
using RestApiProject.Services;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAll()
    {
        return Ok(_bookService.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<Book> GetById(int id)
    {
        var book = _bookService.GetById(id);
        return book is not null ? Ok(book) : NotFound();
    }

    [HttpPost]
    public ActionResult<Book> Create(Book book)
    {
        var created = _bookService.Create(book);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Book updatedBook)
    {
        var updated = _bookService.Update(id, updatedBook);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _bookService.Delete(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpGet("service-id")]
    public IActionResult GetServiceId()
    {
        return Ok(_bookService.GetServiceId());
    }
}