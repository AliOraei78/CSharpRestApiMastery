using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using RestApiProject.Models;
using RestApiProject.Services;
using Xunit;
// Create an alias to avoid conflict with version 2
using ControllerV1 = RestApiProject.Controllers.V1.BooksController;

namespace RestApiProject.Tests
{
    public class BooksControllerV1Tests
    {
        private readonly Mock<IBookService> _mockService;
        private readonly Mock<IValidator<Book>> _mockValidator;
        private readonly ControllerV1 _controller;

        public BooksControllerV1Tests()
        {
            _mockService = new Mock<IBookService>();
            _mockValidator = new Mock<IValidator<Book>>();

            // Inject dependencies into the V1 controller
            _controller = new ControllerV1(_mockService.Object);

            /*
             * Note:
             * If you have not added an IValidator field to the BooksControllerV1 constructor,
             * keep the line above as-is:
             * _controller = new ControllerV1(_mockService.Object);
             */
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithBooks()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book(1, "Book 1", "Author 1", 2020, 100),
                new Book(2, "Book 2", "Author 2", 2021, 200)
            };

            _mockService.Setup(service => service.GetAllAsync())
                        .ReturnsAsync(books);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            // In V1, Select was used to project/shape the output anonymously
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenBookExists()
        {
            // Arrange
            int bookId = 1;
            var book = new Book(bookId, "Book 1", "Author 1", 2020, 100);

            _mockService.Setup(service => service.GetByIdAsync(bookId))
                        .ReturnsAsync(book);

            // Act
            var result = await _controller.GetById(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenDoesNotExist()
        {
            // Arrange
            _mockService.Setup(service => service.GetByIdAsync(It.IsAny<int>()))
                        .ReturnsAsync((Book?)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            int bookId = 1;
            _mockService.Setup(service => service.DeleteAsync(bookId))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(bookId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenKeyNotFoundExceptionOccurs()
        {
            // Arrange
            _mockService.Setup(service => service.DeleteAsync(It.IsAny<int>()))
                        .ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
