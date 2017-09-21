using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel.Client;
using Newtonsoft.Json;
using Swagger.Models;
using Xunit;

namespace IntegrationTests.Tests.Admin
{
    public class ProductsTests
    {
        [Fact]
        public async Task Ok()
        {
            var client = new Swagger.DotnetcoreApiv1(TestConfiguration.ApiUri);

            var disco = await DiscoveryClient.GetAsync(TestConfiguration.IdentityServerUrl);
            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, TestConfiguration.ClientId,
                TestConfiguration.ClientSecret);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            client.HttpClient.SetBearerToken(tokenResponse.AccessToken);

            var getResult = await client.ApiAdminProductsGetWithHttpMessagesAsync();
            getResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            string getContent = await getResult.Response.Content.ReadAsStringAsync();

            var products = JsonConvert.DeserializeObject<IEnumerable<Swagger.Models.Product>>(getContent);
            products.Should().NotBeNullOrEmpty();

            // pick one of the products to get by id

            var selectedProduct = products.FirstOrDefault();
            var getByIdResult = await client.ApiAdminProductsByIdGetWithHttpMessagesAsync(selectedProduct.Id.GetValueOrDefault());
            getByIdResult.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            string getByIdContent = await getByIdResult.Response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<Swagger.Models.AdminProduct>(getByIdContent);
            product.Should().NotBeNull();

            // try and create a new product

            var id = Guid.NewGuid().ToString().Substring(25);

            var newProduct = new CreateProduct();
            newProduct.Color = product.Color;
            //newProduct.DiscontinuedDate = null;
            newProduct.ListPrice = product.ListPrice;
            //newProduct.ModifiedDate // todo remove from create
            newProduct.Name = product.Name + " " + id;
            //newProduct.ProductCategoryId = product.ProductCategoryId;
            //newProduct.ProductModelId = product.ProductModelId;
            newProduct.ProductNumber = id;
            //newProduct.SellEndDate = null;
            newProduct.SellStartDate = DateTime.Now.AddDays(14);
            newProduct.Size = product.Size;
            //newProduct.DiscontinuedDate = null;
            newProduct.StandardCost = product.StandardCost;
            //newProduct.Weight = product.Weight;
            //newProduct.ThumbNailPhoto = product.ThumbNailPhoto;
            //newProduct.ThumbnailPhotoFileName = product.ThumbnailPhotoFileName;

            var createResult = await client.ApiAdminProductsPostWithHttpMessagesAsync(newProduct);
            createResult.Response.StatusCode.Should().Be(HttpStatusCode.Created);
            string createContent = await createResult.Response.Content.ReadAsStringAsync();
            var created = JsonConvert.DeserializeObject<Swagger.Models.CreateProduct>(getByIdContent);
            created.Should().NotBeNull();

            //Location = {
            //    http://localhost:5001/api/admin/products?id=82397ebe-5b70-4643-9cae-c8470c575179}

            var getUrl = createResult.Response.Headers.Location;
            var getId = getUrl.Query.Substring(getUrl.Query.IndexOf('=') + 1);
            Guid createdId = new Guid(getId);

            var getCreated = await client.ApiAdminProductsByIdGetWithHttpMessagesAsync(createdId);
            getCreated.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            string getCreatedContent = await getCreated.Response.Content.ReadAsStringAsync();
            var createdProduct = JsonConvert.DeserializeObject<Swagger.Models.AdminProduct>(getCreatedContent);
            createdProduct.Should().NotBeNull();

            // TODO: verify  contents

            // update the product

            var updateProduct = new UpdateProduct();
            updateProduct.Color = createdProduct.Color;
            updateProduct.ListPrice = createdProduct.ListPrice;
            updateProduct.Name = createdProduct.Name;
            updateProduct.ProductNumber = createdProduct.ProductNumber;
            updateProduct.SellStartDate = createdProduct.SellStartDate;
            updateProduct.Size = createdProduct.Size;
            updateProduct.StandardCost = createdProduct.StandardCost;

            updateProduct.SellEndDate = DateTime.Now.AddDays(30);

            var update =
                await client.ApiAdminProductsByIdPutWithHttpMessagesAsync(createdProduct.Id.GetValueOrDefault(),
                    updateProduct);
            update.Response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // TODO: verify  contents

            var delete = await client.ApiAdminProductsByIdDeleteWithHttpMessagesAsync(createdId);
            delete.Response.StatusCode.Should().Be(HttpStatusCode.OK);

            delete = await client.ApiAdminProductsByIdDeleteWithHttpMessagesAsync(createdId);
            delete.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task NotFound()
        {
            var client = new Swagger.DotnetcoreApiv1(TestConfiguration.ApiUri);

            var disco = await DiscoveryClient.GetAsync(TestConfiguration.IdentityServerUrl);
            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, TestConfiguration.ClientId,
                TestConfiguration.ClientSecret);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            client.HttpClient.SetBearerToken(tokenResponse.AccessToken);
 
            var getByIdResult = await client.ApiAdminProductsByIdGetWithHttpMessagesAsync(Guid.NewGuid());
            getByIdResult.Response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
