using ApiConsumer;

const string apiBaseUrl = "https://localhost:7267/";

var client = new BookClient(apiBaseUrl);

Console.WriteLine("=== Book API Test ===\n");

// 1. Get all books
Console.WriteLine("1. Get all books:");
var books = await client.GetAllBooksAsync();
foreach (var book in books)
{
    Console.WriteLine($"Id: {book.Id} | Title: {book.Title} | Author: {book.Author} | Year: {book.Year} | Price: {book.Price:C}");
}
Console.WriteLine();

// 2. Create new book
Console.WriteLine("2. Create a new book:");
var newBook = new Book(0, "Clean Code", "Robert C. Martin", 2008, 250000m);
var createdBook = await client.CreateBookAsync(newBook);
Console.WriteLine($"New book created: {createdBook.Title} (Id: {createdBook.Id})");
Console.WriteLine();

// 3. Get book by id
Console.WriteLine("3. Get book by Id:");
var bookById = await client.GetBookByIdAsync(createdBook.Id);
if (bookById != null)
{
    Console.WriteLine($"Found: {bookById.Title} by {bookById.Author}");
}
Console.WriteLine();

// 4. Update book
Console.WriteLine("4. Update book:");
bookById!.Price = 300000m;
await client.UpdateBookAsync(bookById.Id, bookById);
Console.WriteLine("Book updated");
Console.WriteLine();

// 5. Delete book
Console.WriteLine("5. Delete book:");
await client.DeleteBookAsync(createdBook.Id);
Console.WriteLine("Book deleted");

// Check the list again
Console.WriteLine("\nFinal list of books:");
books = await client.GetAllBooksAsync();
foreach (var book in books)
{
    Console.WriteLine($"Id: {book.Id} | Title: {book.Title}");
}

Console.ReadKey();
