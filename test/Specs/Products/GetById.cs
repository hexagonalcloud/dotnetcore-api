using System.Threading.Tasks;
using Api.Controllers;
using Api.Controllers.Public;
using Api.Data;
using Api.Models;
using Api.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
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
            var dataMock = new Mock<IProductData>();
            var urlServiceMock = new Mock<IUrlService>();

            dataMock.Setup(data => data.GetById(It.IsAny<int>())).ReturnsAsync(new Product { Name = "Product One" });
            var controller = new ProductsController(dataMock.Object, urlServiceMock.Object);

            var result = await controller.GetById(1) as OkObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);

            var products = result.Value as Product;
            products.Should().NotBeNull();
        }
    }
}
