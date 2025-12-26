using Microsoft.EntityFrameworkCore;
using RestApiProject.Data;
using RestApiProject.Models;
using RestApiProject.Services;
using Xunit;

namespace RestApiProject.Tests
{
    public class BookServiceTests
    {
        // Helper method to create a new DbContext with a unique database name per test
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Random name to avoid conflicts
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated(); // This call adds the seeded data (1, 2, 3)
            return context;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllBooks()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new BookService(context);

            // Act
            var books = await service.GetAllAsync();

            // Assert
            // Since AppDbContext seeds three books, we expect the count to be 3
            Assert.Equal(3, books.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsBook()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new BookService(context);
            int existingId = 1; // This ID exists in the seed data

            // Act
            var book = await service.GetByIdAsync(existingId);

            // Assert
            Assert.NotNull(book);
            Assert.Equal("1984", book.Title);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new BookService(context);

            // Act
            var book = await service.GetByIdAsync(999);

            // Assert
            Assert.Null(book);
        }

        [Fact]
        public async Task CreateAsync_AddsBookCorrectly()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new BookService(context);
            // Use ID 10 to avoid conflicts with 1, 2, and 3
            var newBook = new Book(10, "Clean Code", "Robert C. Martin", 2008, 150000m);

            // Act
            var createdBook = await service.CreateAsync(newBook);

            // Assert
            var bookInDb = await context.Books.FindAsync(10);
            Assert.NotNull(bookInDb);
            Assert.Equal("Clean Code", bookInDb.Title);
            Assert.Equal(4, await context.Books.CountAsync()); // 3 seeded + 1 new
        }

        [Fact]
        public async Task UpdateAsync_ExistingId_UpdatesData()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new BookService(context);
            int idToUpdate = 1;
            var updatedInfo = new Book(idToUpdate, "1984 Updated", "George Orwell", 1950, 130000m);

            // Act
            await service.UpdateAsync(idToUpdate, updatedInfo);

            // Assert
            var bookInDb = await context.Books.FindAsync(idToUpdate);
            Assert.Equal("1984 Updated", bookInDb.Title);
            Assert.Equal(130000m, bookInDb.Price);
        }

        [Fact]
        public async Task DeleteAsync_ExistingId_RemovesBook()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new BookService(context);
            int idToDelete = 2;

            // Act
            await service.DeleteAsync(idToDelete);

            // Assert
            var bookInDb = await context.Books.FindAsync(idToDelete);
            Assert.Null(bookInDb);
            Assert.Equal(2, await context.Books.CountAsync()); // One less book
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new BookService(context);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(999));
        }
    }
}
