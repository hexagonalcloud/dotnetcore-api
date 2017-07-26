using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace RestApi.IntegrationTests.Tests
{
    public class ProductsTests
    {
        [Fact]
        public async Task GetProducts()
        {
            var client = new Swagger.APIV1(TestConfiguration.ApiUri);
            var result = await client.ApiProductsGetWithHttpMessagesAsync();
            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            // TODO: verify the content, we are returning an IActionResult here, slightly different apporach
        }
    }
}