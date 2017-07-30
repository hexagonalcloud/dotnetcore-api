using System;
using System.Collections.Generic;
using Api.Controllers;
using Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
        public void Send_Products_Response()
        {
            var controller = new ProductsController();
            var result = controller.Get() as OkObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);

            var products = result.Value as IEnumerable<Product>;
            products.Should().NotBeNullOrEmpty();
        }
    }
}
