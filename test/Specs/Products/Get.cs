using System;
using System.Collections.Generic;
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
    //- Given a resource Products
    //- When a Client sends a GET request (or API receives a request, pbb makes more sense?)
    //- Then the API sends a Products responseB

    [Trait("Get Products", "")]
    public class Get
    {
        [Fact(DisplayName = "Send Products response")]
        public async Task Send_Products_Response()
        {
            var mockData = new Mock<IProductData>(); 
            mockData.Setup(data => data.Get()).ReturnsAsync(new List<Product>(){ new Product() { Name = "Product One" } });
            var controller = new ProductsController(mockData.Object);

            var result = await controller.Get() as ObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);

            var products = result.Value as IEnumerable<Product>;
            products.Should().NotBeNullOrEmpty();
        }
    }
}
