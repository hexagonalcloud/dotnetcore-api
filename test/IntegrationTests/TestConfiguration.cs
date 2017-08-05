using System;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests
{
    public static class TestConfiguration
    {
        public static Uri ApiUri { get; }
        public static string IdentityServerUrl { get; }
        public static string ClientId { get; }
        public static string ClientSecret { get; }

        public static IConfigurationRoot Configuration { get; }

        static TestConfiguration ()
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
    }
}