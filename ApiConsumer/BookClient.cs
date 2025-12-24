using System.Net.Http.Json;
using System.Text.Json;

namespace ApiConsumer;

public class BookClient
{
    private readonly HttpClient _httpClient;

    public BookClient(string baseAddress)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(baseAddress);
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Book>>("/books") ?? new List<Book>();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<Book>($"/books/{id}");
    }

    public async Task<Book> CreateBookAsync(Book book)
    {
        var response = await _httpClient.PostAsJsonAsync("/books", book);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<Book>())!;
    }

    public async Task UpdateBookAsync(int id, Book book)
    {
        var response = await _httpClient.PutAsJsonAsync($"/books/{id}", book);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteBookAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/books/{id}");
        response.EnsureSuccessStatusCode();
    }
}

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Price { get; set; }

    public Book(int id, string title, string author, int year, decimal price)
    {
        Id = id;
        Title = title;
        Author = author;
        Year = year;
        Price = price;
    }
}