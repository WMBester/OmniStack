using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using WMB.Api.IntegrationTests.Setup;
using WMB.Api.Models;

namespace WMB.Api.IntegrationTests.Tests
{
    internal class CreateAsyncTests : TestFixtureSetup
    {
        private class CreateProductResponse
        {
            public string Message { get; set; } = string.Empty;
            public ProductDto Product { get; set; } = new ProductDto();
        }

        [Test]
        public async Task MyFirstTest()
        {
            // Arrange
            var newProduct = new ProductDto
            {
                Name = "Test Product",
                Price = 9.99m,
                Description = "This is a test product"
            };

            // Act
            var response = await client.PostAsync<ProductDto, CreateProductResponse>("api/Crud", newProduct);

            // Assert
            Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode, "Expected HTTP 201 Created");
            Assert.IsNull(response.ErrorMessage, $"Unexpected error: {response.ErrorMessage}");
            Assert.IsNotNull(response.ResponseBody, "Response body should not be null");
            Assert.AreEqual("Product successfully created", response.ResponseBody.Message);
            Assert.AreEqual(newProduct.Name, response.ResponseBody.Product.Name);
            Assert.AreEqual(newProduct.Price, response.ResponseBody.Product.Price);
            Assert.AreEqual(newProduct.Description, response.ResponseBody.Product.Description);
        }
    }
}
