using Microsoft.AspNetCore.Mvc;
using RestApiProject.Models;
using RestApiProject.Services;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Get all books
    /// </summary>
    /// <returns>List of books</returns>
    /// <response code="200">The list of books was returned successfully</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Book>> GetAll()
    {
        return Ok(_bookService.GetAll());
    }

    /// <summary>
    /// Retrieves a book by its ID
    /// </summary>
    /// <param name="id">The ID of the book</param>
    /// <returns>The book with the specified ID</returns>
    /// <response code="200">Book found</response>
    /// <response code="404">Book not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Book> GetById(int id)
    {
        var book = _bookService.GetById(id);
        return book is not null ? Ok(book) : NotFound();
    }

    /// <summary>
    /// Creates a new book
    /// </summary>
    /// <param name="book">The book to create</param>
    /// <returns>The created book</returns>
    /// <response code="201">Book created successfully</response>
    /// <response code="400">Invalid book data</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Book> Create(Book book)
    {
        var created = _bookService.Create(book);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Updates an existing book
    /// </summary>
    /// <param name="id">The ID of the book to update</param>
    /// <param name="updatedBook">The updated book data</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">Book updated successfully</response>
    /// <response code="404">Book not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, Book updatedBook)
    {
        var updated = _bookService.Update(id, updatedBook);
        return updated ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes a book
    /// </summary>
    /// <param name="id">The ID of the book to delete</param>
    /// <returns>No content if successful</returns>
    /// <response code="204">Book deleted successfully</response>
    /// <response code="404">Book not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var deleted = _bookService.Delete(id);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>
    /// Gets the service instance ID (for testing DI lifetime)
    /// </summary>
    /// <returns>Service hash code</returns>
    [HttpGet("service-id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetServiceId()
    {
        return Ok(_bookService.GetServiceId());
    }
}