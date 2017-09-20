using System.Collections.Generic;
using System.Linq;
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
        public async Task Run()
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

            // TDOO:  no longer applied to public endpoint, add this to an admin integration test
//            var eTagHeader = getByIdResult.Response.Headers.ETag;
//            eTagHeader.IsWeak.Should().BeTrue();

            // now make another request with the etag
            // we expect the result to be empty and the status code to be 304
//            client.HttpClient.DefaultRequestHeaders.IfNoneMatch.Add(eTagHeader);
//
//            var getUnmodifiedByIdResult = await client.ApiPublicProductsByIdGetWithHttpMessagesAsync(selectedProduct.ProductID.GetValueOrDefault());
//            getUnmodifiedByIdResult.Response.StatusCode.Should().Be(HttpStatusCode.NotModified);
//            string unmodifiedResult = await getUnmodifiedByIdResult.Response.Content.ReadAsStringAsync();
//            unmodifiedResult.Should().BeNullOrWhiteSpace();
        }
    }
}
