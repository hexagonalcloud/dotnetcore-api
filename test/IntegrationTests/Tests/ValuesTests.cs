using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace IntegrationTests.Tests
{
    public class ValuesTests
    {
        [Fact]
        public async Task GetValues()
        {
            var client = new Swagger.DotnetcoreApiv1(TestConfiguration.ApiUri);

            var result = await client.ApiPublicValuesGetWithHttpMessagesAsync();
            result.Response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Body.Should().NotBeNullOrEmpty();
        }
    }
}
