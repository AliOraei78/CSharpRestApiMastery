using Microsoft.EntityFrameworkCore;
using RestApiProject.Models;

namespace RestApiProject.Data;

public class AppDbContext : DbContext
{
    public DbSet<Book> Books { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>().HasData(
            new Book(1, "1984", "George Orwell", 1949, 120000m),
            new Book(2, "To Kill a Mockingbird", "Harper Lee", 1960, 150000m),
            new Book(3, "The Great Gatsby", "F. Scott Fitzgerald", 1925, 100000m)
        );
    }
}