using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using RestApiProject.Data;
using RestApiProject.Models;
using RestApiProject.Services;
using Xunit;

namespace RestApiProject.Tests
{
    public class BookServiceTests
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "AllBooksCacheKey"; // Must match the key in BookService

        public BookServiceTests()
        {
            // Setup a real MemoryCache for tests
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            _cache = serviceProvider.GetRequiredService<IMemoryCache>();
        }

        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated(); // Seeds books with IDs 1, 2, 3
            return context;
        }

        // 1. Test GetAll (database + cache)
        [Fact]
        public async Task GetAllAsync_ReturnsAllBooks_AndCachesThem()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new BookService(context, _cache);

            // Act
            var books = await service.GetAllAsync();

            // Assert
            Assert.Equal(3, books.Count);
            Assert.True(_cache.TryGetValue(CacheKey, out _)); // Verify that data is cached
        }

        // 2. Test GetById (no cache logic involved, database only)
        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsBook()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new BookService(context, _cache);

            // Act
            var book = await service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(book);
            Assert.Equal("1984", book.Title);
        }

        // 3. Test Create (database + cache invalidation)
        [Fact]
        public async Task CreateAsync_AddsBook_AndInvalidatesCache()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new BookService(context, _cache);
            await service.GetAllAsync(); // Fill cache first

            // Act
            var newBook = new Book(4, "Clean Architecture", "Uncle Bob", 2017, 200000m);
            await service.CreateAsync(newBook);

            // Assert
            Assert.Equal(4, await context.Books.CountAsync());
            Assert.False(_cache.TryGetValue(CacheKey, out _)); // Cache must be cleared
        }

        // 4. Test Update (database + cache invalidation)
        [Fact]
        public async Task UpdateAsync_UpdatesBook_AndInvalidatesCache()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new BookService(context, _cache);
            await service.GetAllAsync(); // Fill cache

            var updatedInfo = new Book(1, "1984 Updated", "George Orwell", 1950, 130000m);

            // Act
            await service.UpdateAsync(1, updatedInfo);

            // Assert
            var bookInDb = await context.Books.FindAsync(1);
            Assert.Equal("1984 Updated", bookInDb!.Title);
            Assert.False(_cache.TryGetValue(CacheKey, out _)); // Cache must be cleared
        }

        // 5. Test Delete (database + cache invalidation)
        [Fact]
        public async Task DeleteAsync_RemovesBook_AndInvalidatesCache()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new BookService(context, _cache);
            await service.GetAllAsync(); // Fill cache

            // Act
            await service.DeleteAsync(2);

            // Assert
            var bookInDb = await context.Books.FindAsync(2);
            Assert.Null(bookInDb);
            Assert.False(_cache.TryGetValue(CacheKey, out _)); // Cache must be cleared
        }

        // 6. Error test (exception / null case)
        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            using var context = GetDbContext();
            var service = new BookService(context, _cache);

            // Act
            var book = await service.GetByIdAsync(999);

            // Assert
            Assert.Null(book);
        }
    }
}
