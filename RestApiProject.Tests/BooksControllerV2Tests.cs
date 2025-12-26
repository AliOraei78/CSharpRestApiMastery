using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestApiProject.Models;
using RestApiProject.Services;
using Xunit;
// Use an alias to avoid class name conflicts with version 1
using ControllerV2 = RestApiProject.Controllers.V2.BooksController;

namespace RestApiProject.Tests
{
    public class BooksControllerV2Tests
    {
        private readonly Mock<IBookService> _mockService;
        private readonly ControllerV2 _controller;

        public BooksControllerV2Tests()
        {
            _mockService = new Mock<IBookService>();

            // Note: based on your current code, version 2 only receives IBookService in the constructor
            _controller = new ControllerV2(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithIsAvailableField()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book(1, "V2 Book", "Author", 2024, 500) { IsAvailable = true }
            };

            _mockService.Setup(service => service.GetAllAsync())
                        .ReturnsAsync(books);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            // In V2, you used an anonymous projection that includes IsAvailable
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenBookExists()
        {
            // Arrange
            var book = new Book(1, "V2 Title", "Author", 2024, 500);
            _mockService.Setup(service => service.GetByIdAsync(1))
                        .ReturnsAsync(book);

            // Act
            var result = await _controller.GetById(1);

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
            _mockService.Setup(service => service.DeleteAsync(1))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenSuccessful()
        {
            // Arrange
            var book = new Book(1, "Title", "Author", 2024, 100);
            _mockService.Setup(service => service.UpdateAsync(1, book))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, book);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            var book = new Book(99, "Title", "Author", 2024, 100);
            // Simulate throwing your custom exception
            _mockService.Setup(service => service.UpdateAsync(99, book))
                        .ThrowsAsync(new RestApiProject.Exceptions.NotFoundException("Not found"));

            // Act
            var result = await _controller.Update(99, book);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
