# C# REST API Mastery (ASP.NET Core 8/9/10)

A professional REST API project built step-by-step to master modern .NET web development.

## Minimal API Basics

### Features Demonstrated
- **Minimal API** with CRUD endpoints.
- In-memory data storage (List<Book>).
- Full CRUD operations: Get all, Get by ID, Create, Update, Delete.
- OpenAPI/Swagger integration for API documentation and testing.
- HTTPS configuration.

### Key Endpoints
- `GET /books` – Retrieve all books
- `GET /books/{id}` – Retrieve book by ID
- `POST /books` – Create new book
- `PUT /books/{id}` – Update book
- `DELETE /books/{id}` – Delete book

### Model
--- csharp
public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Price { get; set; }
}

## Controllers + Routing + HTTP Verbs

### Features Demonstrated
- Migration from Minimal API to **Controller-based** architecture.
- `[ApiController]` and attribute routing with `[Route("api/[controller]")]`.
- Standard HTTP verbs: `[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]`.
- Action results: `Ok()`, `NotFound()`, `CreatedAtAction()`, `NoContent()`.
- `app.MapControllers()` to enable controller routing.

### Key Endpoints (Controller-based)
- `GET /api/books`
- `GET /api/books/{id}`
- `POST /api/books`
- `PUT /api/books/{id}`
- `DELETE /api/books/{id}`

### Controller Example
--- csharp
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    // CRUD actions with proper HTTP verbs and status codes
}

## Dependency Injection + Services

- Service registration: Singleton, Scoped, Transient.
- Lifetime behavior testing with object IDs.
- Service injection into controllers.
- Refactored CRUD logic into IBookService.