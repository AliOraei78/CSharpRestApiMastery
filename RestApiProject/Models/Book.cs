namespace RestApiProject.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true; // New field for V2
    public Book(int id, string title, string author, int year, decimal price)
    {
        Id = id;
        Title = title;
        Author = author;
        Year = year;
        Price = price;
    }
}