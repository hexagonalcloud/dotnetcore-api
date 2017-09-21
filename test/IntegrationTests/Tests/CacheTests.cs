using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel.Client;
using Swagger.Models;
using Xunit;

namespace IntegrationTests.Tests
{
    public class CacheTests
    {
        [Fact]
        public async Task Run()
        {
            var client = new Swagger.DotnetcoreApiv1(TestConfiguration.ApiUri);

            var disco = await DiscoveryClient.GetAsync(TestConfiguration.IdentityServerUrl);
            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, TestConfiguration.ClientId,
                TestConfiguration.ClientSecret);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            client.HttpClient.SetBearerToken(tokenResponse.AccessToken);

            var product = new Product();
            product.Id = Guid.NewGuid();
            product.Name = "Top top product";
            product.Color = "Yellow";
            product.ListPrice = 100d;
            product.ProductNumber = "not a number";
            product.Size = "M";

            var postRequest = await client.ApiIntegrationCachePostWithHttpMessagesAsync(product);

            postRequest.Response.StatusCode.Should().Be(HttpStatusCode.OK);

            var getRequest = await client.ApiIntegrationCacheByIdGetWithHttpMessagesAsync(product.Id.ToString());

            getRequest.Response.StatusCode.Should().Be(HttpStatusCode.OK);

            // TODO: check response content

            var deleteRequest =
                await client.ApiIntegrationCacheByIdDeleteWithHttpMessagesAsync(product.Id.ToString());

            deleteRequest.Response.StatusCode.Should().Be(HttpStatusCode.OK);

            var validateRequest =
                await client.ApiIntegrationCacheByIdGetWithHttpMessagesAsync(product.Id.ToString());

            validateRequest.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task RunUnauthorized()
        {
            var client = new Swagger.DotnetcoreApiv1(TestConfiguration.ApiUri);

            var product = new Product();
            product.Id = Guid.NewGuid();
            product.Name = "Top top product";
            product.Color = "Yellow";
            product.ListPrice = 100d;
            product.ProductNumber = "not a number";
            product.Size = "M";

            var postRequest = await client.ApiIntegrationCachePostWithHttpMessagesAsync(product);

            postRequest.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var getRequest =
                await client.ApiIntegrationCacheByIdGetWithHttpMessagesAsync(product.Id.ToString());

            getRequest.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            // TODO: check response content

            var deleteRequest =
                await client.ApiIntegrationCacheByIdDeleteWithHttpMessagesAsync(product.Id.ToString());

            deleteRequest.Response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
