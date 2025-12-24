using Microsoft.OpenApi;
using RestApiProject.Middleware;
using RestApiProject.Models;
using RestApiProject.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/api-log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog(); // Replace the default logging system

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Register services with different lifetimes
builder.Services.AddSingleton<IBookService, BookService>();   // Singleton (one instance for the entire application)
builder.Services.AddScoped<IBookService, BookService>();      // Scoped (one instance per HTTP request)
builder.Services.AddTransient<IBookService, BookService>();   // Transient (a new instance every time)

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BookStore API",
        Version = "v1",
        Description = "Simple Api",
        Contact = new OpenApiContact
        {
            Name = "Ali Jenabi",
            Email = "a.jenabi78@example.com"
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}

app.UseHttpsRedirection();

// Custom middleware
app.UseRequestTiming();

app.MapControllers();
/*
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

var books = new List<Book>
{
    new Book { Id = 1, Title = "1984", Author = "George Orwell", Year = 1949, Price = 120000m },
    new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", Year = 1960, Price = 150000m },
    new Book { Id = 3, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Year = 1925, Price = 100000m }
};

// GET all books
app.MapGet("/books", () => books)
   .WithName("GetAllBooks");

// GET book by id
app.MapGet("/books/{id}", (int id) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);
    return book is not null ? Results.Ok(book) : Results.NotFound();
})
.WithName("GetBookById");

// POST new book
app.MapPost("/books", (Book book) =>
{
    book.Id = books.Max(b => b.Id) + 1;
    books.Add(book);
    return Results.Created($"/books/{book.Id}", book);
})
.WithName("CreateBook");

// PUT update book
app.MapPut("/books/{id}", (int id, Book updatedBook) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);
    if (book is null) return Results.NotFound();

    book.Title = updatedBook.Title;
    book.Author = updatedBook.Author;
    book.Year = updatedBook.Year;
    book.Price = updatedBook.Price;

    return Results.NoContent();
})
.WithName("UpdateBook");

// DELETE book
app.MapDelete("/books/{id}", (int id) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);
    if (book is null) return Results.NotFound();

    books.Remove(book);
    return Results.NoContent();
})
.WithName("DeleteBook");
*/
app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
