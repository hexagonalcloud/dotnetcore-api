using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace RestApi.IntegrationTests
{
    public class ValuesIntegrationTests
    {
        [Fact]
        public async Task GetValues()
        {
            var client = new Swagger.APIV1();
            var result = await client.ApiValuesGetWithHttpMessagesAsync();
            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Body.Should().NotBeNullOrEmpty();
        }
    }
}
