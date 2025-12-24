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
    /// Retrieves all books
    /// </summary>
    /// <returns>A list of books</returns>
    /// <response code="200">Returns the list of books</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Book>>> GetAll()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
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
    public async Task<ActionResult<Book>> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
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
    public async Task<ActionResult<Book>> Create(Book book)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _bookService.CreateAsync(book);
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
    public async Task<IActionResult> Update(int id, Book updatedBook)
    {
        var updated = await _bookService.UpdateAsync(id, updatedBook);
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
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _bookService.DeleteAsync(id);
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