
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using WMB.Api.DbContext;
using WMB.Api.Models;
using WMB.Api.Services;
using EntityFrameworkCore.Testing.Moq;
using System.Data.Common;

namespace WMB.Api.Tests.tests
{
    public class UpdateAsyncTests : TestFixtureBase
    {

        [Test]
        public async Task Given_ValidIdAndProductDto_When_UpdateAsyncCalled_Then_ProductUpdatedSuccessfully()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Old Name", Price = 10, Description = "Old Desc" };
            _mockContext.Products.Add(product);
            _mockContext.SaveChanges();

            var productDto = new ProductDto { Name = "New Name", Price = 20, Description = "New Desc" };

            // Act
            var result = await _crudService.UpdateAsync(1, productDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
            var objectResult = result.Result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(objectResult.StatusCode, Is.EqualTo(200));
                Assert.That(objectResult.Value.ToString(), Does.Contain("Update successful"));
            });
        }

        [Test]
        public async Task Given_IdLessThanOrEqualZero_When_UpdateAsyncCalled_Then_ReturnsBadRequest()
        {
            // Arrange
            var productDto = new ProductDto { Name = "Name", Price = 10, Description = "Desc" };

            // Act
            var result = await _crudService.UpdateAsync(0, productDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.That(badRequest.Value, Is.EqualTo("Id must be greater than zero"));
        }

        [Test]
        public async Task Given_ProductNotFound_When_UpdateAsyncCalled_Then_ReturnsNotFound()
        {
            // Arrange
            var productDto = new ProductDto { Name = "Name", Price = 10, Description = "Desc" };

            // Act
            var result = await _crudService.UpdateAsync(999, productDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            var notFound = result.Result as NotFoundObjectResult;
            Assert.That(notFound.Value, Is.EqualTo("Product with id 999 not found"));
        }

        [Test]
        public async Task Given_DbExceptionThrown_When_UpdateAsyncCalled_Then_ReturnsInternalServerError()
        {
            // Arrange
            var productDto = new ProductDto { Name = "Name", Price = 10, Description = "Desc" };

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            mockContext.Setup(m => m.Products.FindAsync(It.IsAny<int>())).ReturnsAsync(new Product());
            mockContext.Setup(m => m.SaveChangesAsync(default)).Throws(new Exception("An error occurred while updating the product."));

            var service = new CrudService(mockContext.Object);

            // Act
            var result = await service.UpdateAsync(1, productDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
            var objectResult = result.Result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(objectResult.StatusCode, Is.EqualTo(500));
                Assert.That(objectResult.Value.ToString(), Does.Contain("An error occurred while updating the product."));
            });
        }

        [Test]
        public async Task Given_GenericExceptionThrown_When_UpdateAsyncCalled_Then_ReturnsInternalServerError()
        {
            // Arrange
            var productDto = new ProductDto { Name = "Name", Price = 10, Description = "Desc" };

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            mockContext.Setup(m => m.Products.FindAsync(It.IsAny<int>())).ReturnsAsync(new Product());
            mockContext.Setup(m => m.SaveChangesAsync(default)).Throws(new Exception("An error occurred while updating the product."));

            var service = new CrudService(mockContext.Object);

            // Act
            var result = await service.UpdateAsync(1, productDto);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
            var objectResult = result.Result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(objectResult.StatusCode, Is.EqualTo(500));
                Assert.That(objectResult.Value.ToString(), Does.Contain("An error occurred while updating the product."));
            });
        }
    }
}
