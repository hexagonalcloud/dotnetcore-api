using System;
using System.Collections.Generic;
using System.Linq;
using Api.Controllers;
using Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Specs.Products
{
    [Trait("Get Product By Id", "")]
    public class GetById
    {
        [Fact(DisplayName = "Send Single Product response")]
        public void Send_Product_Response()
        {
            var controller = new ProductsController();
            var result = controller.Get("one") as OkObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);

            var products = result.Value as Product;
            products.Should().NotBeNull();
        }
    }
}
