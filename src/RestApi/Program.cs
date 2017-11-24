using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using RestApi.Logging;
using Serilog;
using Serilog.Exceptions;

namespace RestApi
{
    public class Program
    {
        private static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);
            Configuration = builder.Build();

            // TODO: initialize application insights telemetry here if we have a key in the config

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithThreadId()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithExceptionDetails()
                .Enrich.With<ApplicationDetailsEnricher>()
                .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                BuildWebHost(args)
                    .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(Configuration)
                .ConfigureServices(services => services.AddAutofac())
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
    }
}
