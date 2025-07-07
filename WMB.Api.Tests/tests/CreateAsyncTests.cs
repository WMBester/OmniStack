using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using WMB.Api.DbContext;
using WMB.Api.Models;
using WMB.Api.Services;
using WMB.Api.Tests;

namespace WMB.Api.Unit.Tests.Tests
{
    public class CreateAsyncTests : TestFixtureBase
    {

        [Test]
        public async Task Given_ValidProduct_When_CreateAsyncCalled_Then_ProductCreatedSuccessfully()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "Test Product",
                Price = 60,
                Description = "A product for testing"
            };

            // Act
            var result = await _crudService.CreateProductAsync(productDto);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(objectResult.StatusCode, Is.EqualTo(201));
                Assert.That(objectResult.Value.ToString(), Does.Contain("Product successfully created"));
            });
        }

        [Test]
        public async Task Given_NullProductDto_When_CreateAsyncCalled_Then_BadRequestReturned()
        {
            // Arrange
            ProductDto productDto = null;

            // Act
            var result = await _crudService.CreateProductAsync(productDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest.Value, Is.EqualTo("Product data is required."));

        }

        [Test]
        public async Task Given_ArgumentExceptionThrown_When_CreateAsyncCalled_Then_BadRequestReturned()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "Test Product",
                Price = 60,
                Description = "A product for testing"
            };

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            mockContext.Setup(m => m.Products.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                       .Throws(new ArgumentException("Invalid argument"));

            var service = new CrudService(mockContext.Object);

            // Act
            var result = await service.CreateProductAsync(productDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest.Value, Is.EqualTo("Invalid argument"));
        }

        [Test]
        public async Task Given_DbUpdateExceptionThrown_When_CreateAsyncCalled_Then_InternalServerErrorReturned()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "Test Product",
                Price = 60,
                Description = "A product for testing"
            };

            var dbUpdateException = new DbUpdateException("DB update failed", new Exception("Inner exception"));

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            mockContext.Setup(m => m.Products.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                       .Throws(dbUpdateException);

            var service = new CrudService(mockContext.Object);

            // Act
            var result = await service.CreateProductAsync(productDto);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(objectResult.StatusCode, Is.EqualTo(500));
                Assert.That(objectResult.Value.ToString(), Does.Contain("DB update failed"));
                Assert.That(objectResult.Value.ToString(), Does.Contain("Inner exception"));
            });
        }

        [Test]
        public async Task Given_DbUpdateExceptionWithoutInnerThrown_When_CreateAsyncCalled_Then_InternalServerErrorReturned()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "Test Product",
                Price = 60,
                Description = "A product for testing"
            };

            var dbUpdateException = new DbUpdateException("DB update failed");

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            mockContext.Setup(m => m.Products.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                       .Throws(dbUpdateException);

            var service = new CrudService(mockContext.Object);

            // Act
            var result = await service.CreateProductAsync(productDto);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(objectResult.StatusCode, Is.EqualTo(500));
                Assert.That(objectResult.Value.ToString(), Does.Contain("DB update failed"));
            });
        }

        [Test]
        public async Task Given_GenericExceptionThrown_When_CreateAsyncCalled_Then_InternalServerErrorReturned()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "Test Product",
                Price = 60,
                Description = "A product for testing"
            };

            var genericException = new Exception("Something went wrong", new Exception("Inner exception"));

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            mockContext.Setup(m => m.Products.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                       .Throws(genericException);

            var service = new CrudService(mockContext.Object);

            // Act
            var result = await service.CreateProductAsync(productDto);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(objectResult.StatusCode, Is.EqualTo(500));
                Assert.That(objectResult.Value.ToString(), Does.Contain("An error occurred while creating the product."));
                Assert.That(objectResult.Value.ToString(), Does.Contain("Inner exception"));
            });
        }

        [Test]
        public async Task Given_GenericExceptionWithoutInnerThrown_When_CreateAsyncCalled_Then_InternalServerErrorReturned()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "Test Product",
                Price = 60,
                Description = "A product for testing"
            };

            var genericException = new Exception("Something went wrong");

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            mockContext.Setup(m => m.Products.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                       .Throws(genericException);

            var service = new CrudService(mockContext.Object);

            // Act
            var result = await service.CreateProductAsync(productDto);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(objectResult.StatusCode, Is.EqualTo(500));
                Assert.That(objectResult.Value.ToString(), Does.Contain("An error occurred while creating the product."));
            });
        }
    }
}
