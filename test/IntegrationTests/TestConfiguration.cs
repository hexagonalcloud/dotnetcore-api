using System;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests
{
    public static class TestConfiguration
    {
        static TestConfiguration()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json", optional: false)
                .AddJsonFile($"testsettings.{env}.json", optional: true);
            Configuration = builder.Build();

            ApiUri = new Uri(Configuration["RestApi"]);
            IdentityServerUrl = Configuration["IdentityServer"];
            ClientId = Configuration["ClientId"];
            ClientSecret = Configuration["ClientSecret"];
        }

        public static Uri ApiUri { get; }

        public static string IdentityServerUrl { get; }

        public static string ClientId { get; }

        public static string ClientSecret { get; }

        public static IConfigurationRoot Configuration { get; }
    }
}
