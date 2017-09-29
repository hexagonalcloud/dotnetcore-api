using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTests.Tests.Public
{
    public class ProductsTests
    {
        [Fact]
        public async Task Ok()
        {
            var client = new Swagger.DotnetcoreApiv1(TestConfiguration.ApiUri);
            var getResult = await client.ApiPublicProductsGetWithHttpMessagesAsync();
            getResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            string getContent = await getResult.Response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Swagger.Models.Product>>(getContent);
            products.Should().NotBeNullOrEmpty();

            // pick one of the products to get by id
            var selectedProduct = products.FirstOrDefault();
            var getByIdResult = await client.ApiPublicProductsByIdGetWithHttpMessagesAsync(selectedProduct.Id.GetValueOrDefault());
            getByIdResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            string getByIdContent = await getByIdResult.Response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Swagger.Models.Product>(getByIdContent);
            product.Should().NotBeNull();
        }

        [Fact]
        public async Task NotFound()
        {
            var client = new Swagger.DotnetcoreApiv1(TestConfiguration.ApiUri);
            var getByIdResult = await client.ApiPublicProductsByIdGetWithHttpMessagesAsync(Guid.NewGuid());
            getByIdResult.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
