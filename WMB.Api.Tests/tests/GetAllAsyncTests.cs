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
    public class GetAllAsyncTests : TestFixtureBase
    {

        [Test]
        public async Task Given_ProductsExist_When_GetAllAsyncCalled_Then_ReturnsOkObjectResultWithProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Price = 10, Description = "Desc 1" },
                new Product { Id = 2, Name = "Product 2", Price = 20, Description = "Desc 2" }
            };

            // Add test data to the mocked context
            _mockContext.Products.AddRange(products);
            _mockContext.SaveChanges();

            // Act
            var result = await _crudService.GetAllAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<List<Product>>>());
            var actionResult = result.Result as OkObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(actionResult, Is.Not.Null);
                Assert.That(((List<Product>)actionResult.Value).Count, Is.EqualTo(products.Count));
            });
        }

        [Test]
        public async Task Given_ExceptionThrown_When_GetAllAsyncCalled_Then_ReturnsInternalServerError()
        {
            // Arrange
            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            mockContext.Setup(m => m.Products).Throws(new Exception("An error occurred while retrieving the products."));

            var service = new CrudService(mockContext.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<List<Product>>>());
            var actionResult = result.Result as ObjectResult;
            Assert.That(actionResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actionResult.StatusCode, Is.EqualTo(500));
                Assert.That(actionResult.Value.ToString(), Does.Contain("An error occurred while retrieving the products."));
            });
        }

        [Test]
        public async Task Given_GenericExceptionThrown_When_GetAllAsyncCalled_Then_ReturnsInternalServerError()
        {
            // Arrange
            var genericException = new Exception("Generic error", new Exception("Inner exception"));

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            mockContext.Setup(m => m.Products).Throws(genericException);

            var service = new CrudService(mockContext.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<List<Product>>>());
            var actionResult = result.Result as ObjectResult;
            Assert.That(actionResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actionResult.StatusCode, Is.EqualTo(500));
                Assert.That(actionResult.Value.ToString(), Does.Contain("Inner exception"));
            });
        }

        [Test]
        public async Task Given_GenericExceptionWithoutInnerThrown_When_GetAllAsyncCalled_Then_ReturnsInternalServerError()
        {
            // Arrange
            var genericException = new Exception("An error occurred while retrieving the products.");

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            mockContext.Setup(m => m.Products).Throws(genericException);

            var service = new CrudService(mockContext.Object);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<ActionResult<List<Product>>>());
            var actionResult = result.Result as ObjectResult;
            Assert.That(actionResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actionResult.StatusCode, Is.EqualTo(500));
                Assert.That(actionResult.Value.ToString(), Does.Contain("An error occurred while retrieving the products."));
            });
        }
    }
}
