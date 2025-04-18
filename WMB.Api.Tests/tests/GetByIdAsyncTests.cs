﻿﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WMB.Api.DbContext;
using WMB.Api.Models;
using WMB.Api.Services;
using EntityFrameworkCore.Testing.Moq;

namespace WMB.Api.Tests.tests
{
    public class GetByIdAsyncTests : TestFixtureBase
    {

        [Test]
        public async Task Given_ValidId_When_GetByIdAsyncCalled_Then_ReturnsProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Price = 10, Description = "Desc 1" };
            _mockContext.Products.Add(product);
            _mockContext.SaveChanges();

            // Act
            var result = await _crudService.GetByIdAsync(1);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
            var actionResult = result.Result as OkObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(actionResult, Is.Not.Null);
                Assert.That(((Product)actionResult.Value).Id, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task Given_IdLessThanOrEqualZero_When_GetByIdAsyncCalled_Then_ReturnsBadRequest()
        {
            // Act
            var result = await _crudService.GetByIdAsync(0);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.That(badRequest.Value, Is.EqualTo("Id must be greater than zero"));
        }

        [Test]
        public async Task Given_ProductNotFound_When_GetByIdAsyncCalled_Then_ReturnsNotFound()
        {
            // Act
            var result = await _crudService.GetByIdAsync(999);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
            var notFound = result.Result as NotFoundObjectResult;
            Assert.That(notFound.Value, Is.EqualTo("Product with id 999 not found"));
        }

        [Test]
        public async Task Given_ExceptionThrown_When_GetByIdAsyncCalled_Then_ReturnsInternalServerError()
        {
            // Arrange
            var mockContext = Create.MockedDbContextFor<ApplicationDbContext>();
            var mockSet = mockContext.Products;

            var mockSetMock = Mock.Get(mockSet);
            mockSetMock.Setup(m => m.FindAsync(It.IsAny<int>())).Throws(new Exception("An error occurred while retrieving the product."));

            var service = new CrudService(mockContext);

            // Act
            var result = await service.GetByIdAsync(1);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
            var objectResult = result.Result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(objectResult.StatusCode, Is.EqualTo(500));
                Assert.That(objectResult.Value.ToString(), Does.Contain("An error occurred while retrieving the product."));
            });
        }
    }
}
