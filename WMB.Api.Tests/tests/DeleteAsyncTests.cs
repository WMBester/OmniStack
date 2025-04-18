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
    public class DeleteAsyncTests : TestFixtureBase
    {

        [Test]
        public async Task Given_ValidId_When_DeleteAsyncCalled_Then_ProductDeletedSuccessfully()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product to Delete", Price = 10, Description = "Desc" };
            _mockContext.Products.Add(product);
            _mockContext.SaveChanges();

            // Act
            var result = await _crudService.DeleteAsync(1);

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public async Task Given_IdLessThanOrEqualZero_When_DeleteAsyncCalled_Then_ReturnsBadRequest()
        {
            // Act
            var result = await _crudService.DeleteAsync(0);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequest = result as BadRequestObjectResult;
            Assert.That(badRequest.Value, Is.EqualTo("Id must be greater than zero"));
        }

        [Test]
        public async Task Given_ProductNotFound_When_DeleteAsyncCalled_Then_ReturnsNotFound()
        {
            // Act
            var result = await _crudService.DeleteAsync(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFound = result as NotFoundObjectResult;
            Assert.That(notFound.Value, Is.EqualTo("Product with id 999 not found"));
        }

        [Test]
        public async Task Given_DbExceptionThrown_When_DeleteAsyncCalled_Then_ReturnsInternalServerError()
        {
            // Arrange
            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            mockContext.Setup(m => m.Products.FindAsync(It.IsAny<int>())).ReturnsAsync(new Product());
            mockContext.Setup(m => m.SaveChangesAsync(default)).Throws(new Exception("An error occurred while retrieving the product."));

            var service = new CrudService(mockContext.Object);

            // Act
            var result = await service.DeleteAsync(1);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(objectResult.StatusCode, Is.EqualTo(500));
                Assert.That(objectResult.Value.ToString(), Does.Contain("An error occurred while deleting the product."));
            });
        }

        [Test]
        public async Task Given_GenericExceptionThrown_When_DeleteAsyncCalled_Then_ReturnsInternalServerError()
        {
            // Arrange
            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            mockContext.Setup(m => m.Products.FindAsync(It.IsAny<int>())).ReturnsAsync(new Product());
            mockContext.Setup(m => m.SaveChangesAsync(default)).Throws(new Exception("An error occurred while deleting the product"));

            var service = new CrudService(mockContext.Object);

            // Act
            var result = await service.DeleteAsync(1);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(objectResult.StatusCode, Is.EqualTo(500));
                Assert.That(objectResult.Value.ToString(), Does.Contain("An error occurred while deleting the product."));
            });
        }
    }
}
