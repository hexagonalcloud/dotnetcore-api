using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using IntegrationTests.Simple;
using Microsoft.Rest;
using Newtonsoft.Json;
using Swagger.Public;
using Swagger.Public.Models;
using Xunit;

namespace IntegrationTests.Tests.Public
{
    public class Products
    {
        private static readonly PublicAdventureAPI Client = new PublicAdventureAPI(TestConfiguration.ApiUri);

        [Trait("Get without parameters", "")]
        public class GetWithoutParameters : AsyncSpec
        {
            private HttpOperationResponse<IList<Product>> _response;

            [When("Sendiing a request without parameters")]
            public async Task When()
            {
                _response = await Client.ApiPublicProductsGetWithHttpMessagesAsync();
            }

            [Then(DisplayName = "Returns a 200 StatusCode")]
            public void Returns_200()
            {
                _response.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            [Then (DisplayName = "Returns a link in the header")]
            public void Returns_LinkHeader()
            {
                var linkHeader = _response.Response.Headers.FirstOrDefault(m => m.Key.Equals("Link"));
                linkHeader.Should().NotBeNull();
            }

            [Then(DisplayName = "Returns 10 products")]
            public async Task Returns_10_Products()
            {
                string getContent = await _response.Response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<IEnumerable<Swagger.Models.Product>>(getContent).ToList();
                products.Should().NotBeNullOrEmpty();
                products.Count().Should().Be(10);
            }
        }

        [Trait("", "")]
        public class GetByIdWithExisingId : AsyncSpec
        {
            private HttpOperationResponse<Product> _response;
            private Guid _productId;
 
            [Given("An existing product ID")]
            public async Task Given()
            {
                var response = await Client.ApiPublicProductsGetWithHttpMessagesAsync();
                string getContent = await response.Response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<IEnumerable<Swagger.Models.Product>>(getContent).ToList();
                _productId = products.First().Id.GetValueOrDefault();
            }

            [When("Sendiing a request")]
            public async Task When()
            {
                _response = await Client.ApiPublicProductsByIdGetWithHttpMessagesAsync(_productId);
            }

            [Then(DisplayName = "Returns 200 StatusCode")]
            public void Returns_200()
            {
                _response.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            }

            [Then(DisplayName = "Returns Prodcuct")]
            public async Task Returns_Product()
            {
                string getContent = await _response.Response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Swagger.Models.Product>(getContent);

                product.Should().NotBeNull();
                product.Id.HasValue.Should().BeTrue();
                product.Id.Should().Be(_productId);
            } 
        }

        [Trait("", "")]
        public class GetByIdWithUnknownId : AsyncSpec
        {
            private HttpOperationResponse<Product> _response;

            [When("Sendiing a request with an unknown ID")]
            public async Task When()
            {
                _response = await Client.ApiPublicProductsByIdGetWithHttpMessagesAsync(Guid.NewGuid());
            }

            [Then(DisplayName = "Returns a 404 StatusCode")]
            public void Returns_404()
            {
                _response.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }
    }
}
