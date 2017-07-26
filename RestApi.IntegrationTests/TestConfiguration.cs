using System;
using Microsoft.Extensions.Configuration;

namespace RestApi.IntegrationTests
{
    public static class TestConfiguration
    {
        public static Uri ApiUri { get; set; }
        public static IConfigurationRoot Configuration { get; }

        static TestConfiguration ()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                       .AddJsonFile("testsettings.json", optional: false)
                       .AddJsonFile($"testsettings.{env}.json", optional: true);
            Configuration = builder.Build();
            ApiUri = new Uri(Configuration["RestApi"]);
        }
    }
}