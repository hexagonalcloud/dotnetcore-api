using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel.Client;
using Newtonsoft.Json;
using Swagger.Admin;
using Swagger.Admin.Models;
using Xunit;

namespace IntegrationTests.Tests.Admin
{
    public class ProductsTests
    {
        [Fact]
        public async Task Ok()
        {
            var client = await CreateAuthenticatedAutoRestClient();

            var getResult = await client.ApiAdminProductsGetWithHttpMessagesAsync();
            getResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            string getContent = await getResult.Response.Content.ReadAsStringAsync();

            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(getContent);
            products.Should().NotBeNullOrEmpty();

            // pick one of the products to get by id
            var selectedProduct = products.FirstOrDefault();
            var getByIdResult = await client.ApiAdminProductsByIdGetWithHttpMessagesAsync(selectedProduct.Id.GetValueOrDefault());
            getByIdResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            string getByIdContent = await getByIdResult.Response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Swagger.Admin.Models.AdminProduct>(getByIdContent);
            product.Should().NotBeNull();

            // try and create a new product
            var id = Guid.NewGuid().ToString().Substring(25);

            var newProduct = new CreateProduct();
            newProduct.Color = product.Color;
            newProduct.ListPrice = product.ListPrice.GetValueOrDefault();
            newProduct.Name = product.Name + " " + id;
            newProduct.ProductNumber = id;
            newProduct.SellStartDate = DateTime.Now.AddDays(14);
            newProduct.Size = product.Size;
            newProduct.StandardCost = product.StandardCost.GetValueOrDefault();
            var createResult = await client.ApiAdminProductsPostWithHttpMessagesAsync(newProduct);
            createResult.Response.StatusCode.Should().Be(HttpStatusCode.Created);
            string createContent = await createResult.Response.Content.ReadAsStringAsync();
            var created = JsonConvert.DeserializeObject<Swagger.Admin.Models.CreateProduct>(getByIdContent);
            created.Should().NotBeNull();

            // Location = {
            // http://localhost:5001/api/admin/products?id=82397ebe-5b70-4643-9cae-c8470c575179}
            var getUrl = createResult.Response.Headers.Location;
            var getId = getUrl.Query.Substring(getUrl.Query.IndexOf('=') + 1);
            Guid createdId = new Guid(getId);

            var getCreated = await client.ApiAdminProductsByIdGetWithHttpMessagesAsync(createdId);
            getCreated.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            string getCreatedContent = await getCreated.Response.Content.ReadAsStringAsync();
            var createdProduct = JsonConvert.DeserializeObject<Swagger.Admin.Models.AdminProduct>(getCreatedContent);
            createdProduct.Should().NotBeNull();

            // TODO: verify  contents

            // update the product
            var updateProduct = new UpdateProduct();
            updateProduct.Color = createdProduct.Color;
            updateProduct.ListPrice = createdProduct.ListPrice.GetValueOrDefault();
            updateProduct.Name = createdProduct.Name;
            updateProduct.ProductNumber = createdProduct.ProductNumber;
            updateProduct.SellStartDate = createdProduct.SellStartDate.GetValueOrDefault();
            updateProduct.Size = createdProduct.Size;
            updateProduct.StandardCost = createdProduct.StandardCost.GetValueOrDefault();

            updateProduct.SellEndDate = DateTime.Now.AddDays(30);

            var update =
                await client.ApiAdminProductsByIdPutWithHttpMessagesAsync(createdProduct.Id.GetValueOrDefault(), updateProduct);
            update.Response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // TODO: verify  contents
            var patch = new Operation("Yellow", "/color", "replace");
            var patches = new List<Operation>();
            patches.Add(patch);

            var pathcProduct =
                await client.ApiAdminProductsByIdPatchWithHttpMessagesAsync(createdProduct.Id.GetValueOrDefault(), patches);

            pathcProduct.Response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // TODO : verify contents
            var delete = await client.ApiAdminProductsByIdDeleteWithHttpMessagesAsync(createdId);
            delete.Response.StatusCode.Should().Be(HttpStatusCode.OK);

            delete = await client.ApiAdminProductsByIdDeleteWithHttpMessagesAsync(createdId);
            delete.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task NotFound()
        {
            var client = await CreateAuthenticatedAutoRestClient();

            var getByIdResult = await client.ApiAdminProductsByIdGetWithHttpMessagesAsync(Guid.NewGuid());
            getByIdResult.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            // TODO: other verbs
        }

        [Fact]
        public async Task UnprocessableEntity()
        {
            var httpClient = await CreateAuthenticatedHttpClient();

            var newProduct = new CreateProduct();
            var jsonContent = JsonConvert.SerializeObject(newProduct);

            // POST
            var postResult = await httpClient.PostAsync("api/admin/products", new StringContent(jsonContent, Encoding.UTF8, "application/json"));
            postResult.StatusCode.Should().Be((HttpStatusCode)422);

            // PUT
            var autorestClient = await CreateAuthenticatedAutoRestClient();

            var getResult = await autorestClient.ApiAdminProductsGetWithHttpMessagesAsync();
            getResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            string getContent = await getResult.Response.Content.ReadAsStringAsync();

            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(getContent);
            var selectedProduct = products.FirstOrDefault();

            var putResult = await httpClient.PutAsync("api/admin/products/" + selectedProduct.Id.GetValueOrDefault(), new StringContent(jsonContent, Encoding.UTF8, "application/json"));
            putResult.StatusCode.Should().Be((HttpStatusCode)422);

            // PATCH
            var patch = new Operation(string.Empty, "/name", "replace");
            var patches = new List<Operation>();
            patches.Add(patch);

            var jsonPatch = JsonConvert.SerializeObject(patches);

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), "api/admin/products/" + selectedProduct.Id.GetValueOrDefault());
            request.Content = new StringContent(jsonPatch, Encoding.UTF8, "application/json-patch+json");

            var patchResult = await httpClient.SendAsync(request);

            patchResult.StatusCode.Should().Be((HttpStatusCode)422);
        }

        private static async Task<HttpClient> CreateAuthenticatedHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = TestConfiguration.ApiUri;
            var disco = await DiscoveryClient.GetAsync(TestConfiguration.IdentityServerUrl);

            var tokenClient = new TokenClient(disco.TokenEndpoint, TestConfiguration.ClientId, TestConfiguration.ClientSecret);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            client.SetBearerToken(tokenResponse.AccessToken);
            return client;
        }

        private static async Task<AdminAdventureAPI> CreateAuthenticatedAutoRestClient()
        {
            var client = new Swagger.Admin.AdminAdventureAPI(TestConfiguration.ApiUri);

            var disco = await DiscoveryClient.GetAsync(TestConfiguration.IdentityServerUrl);
            var tokenClient = new TokenClient(disco.TokenEndpoint, TestConfiguration.ClientId, TestConfiguration.ClientSecret);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            client.HttpClient.SetBearerToken(tokenResponse.AccessToken);
            return client;
        }
    }
}
