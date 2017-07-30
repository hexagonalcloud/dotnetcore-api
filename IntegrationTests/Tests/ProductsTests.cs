using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace IntegrationTests.Tests
{
    public class ProductsTests
    {
        [Fact]
        public async Task GetProducts()
        {
            var client = new Swagger.APIV1(TestConfiguration.ApiUri);
            var result = await client.ApiProductsGetWithHttpMessagesAsync();
            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            string content = await result.Response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<Swagger.Models.Product>>(content);
            products.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetProduct()
        {
            var client = new Swagger.APIV1(TestConfiguration.ApiUri);
            var result = await client.ApiProductsByIdGetWithHttpMessagesAsync("one");
            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
              string content = await result.Response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Swagger.Models.Product>(content);
            product.Should().NotBeNull();
        }
    }
}