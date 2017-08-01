using System.Threading.Tasks;
using Api.Controllers;
using Api.Data;
using Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Specs.Products
{
    [Trait("Get Product By Id", "")]
    public class GetById
    {
        [Fact(DisplayName = "Send Single Product response")]
        public async Task Send_Product_Response()
        {
            var mockData = new Mock<IProductData>();
            mockData.Setup(data => data.GetById(It.IsAny<int>())).ReturnsAsync(new Product{ Name = "Product One" });
            var controller = new ProductsController(mockData.Object);

            var result = await controller.Get(1) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);

            var products = result.Value as Product;
            products.Should().NotBeNull();
        }
    }
}
