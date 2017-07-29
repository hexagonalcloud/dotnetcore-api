using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace RestApi.IntegrationTests.Tests
{
    public class ValuesTests
    {
        [Fact]
        public async Task GetValues()
        {
            var client = new Swagger.APIV1(TestConfiguration.ApiUri);
            
            var result = await client.ApiValuesGetWithHttpMessagesAsync();
            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Body.Should().NotBeNullOrEmpty();
        }
    }
}
