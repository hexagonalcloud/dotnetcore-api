using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Controllers;
using Api.Controllers.Public;
using Api.Data;
using Api.Models;
using Api.Parameters;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
            var dataMock = new Mock<IProductData>();
            var urlHelperMock = new Mock<IUrlHelper>();
            var httpContextMock = new Mock<HttpContext>();

            var pagedList = new PagedList<Product>(new List<Product>() { new Product() { Name = "Product One" } }, 1, new PagingParameters(), new FilterParameters());
            dataMock.Setup(data => data.Get(new PagingParameters(), new FilterParameters())).ReturnsAsync(pagedList);

            httpContextMock.Setup(context => context.Response.Headers).Returns(new HeaderDictionary());

            //var controller = new ProductsController(dataMock.Object, urlHelperMock.Object);
            //controller.ControllerContext = new ControllerContext();
            //controller.ControllerContext.HttpContext = httpContextMock.Object;

            //var result = await controller.Get(new PagingParameters(), new FilterParameters()) as ObjectResult;

            //result.Should().NotBeNull();
            //result.StatusCode.Should().Be(200);

            //var products = result.Value as IEnumerable<Product>;
            //products.Should().NotBeNullOrEmpty();
        }
    }
}
