using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace RestApi.IntegrationTests
{
    public class ValuesIntegrationTests
    {
        public IConfigurationRoot Configuration { get; }

        public ValuesIntegrationTests()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                       .AddJsonFile("testsettings.json", optional: false)
                       .AddJsonFile($"testsettings.{env}.json", optional: true);
            Configuration = builder.Build();
        }

        [Fact]
        public async Task GetValues()
        {
            var apiUri = new Uri(Configuration["RestApi"]);
            var client = new Swagger.APIV1(apiUri);
            
            var result = await client.ApiValuesGetWithHttpMessagesAsync();
            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Body.Should().NotBeNullOrEmpty();
        }
    }
}
