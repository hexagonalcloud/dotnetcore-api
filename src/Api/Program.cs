using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Api
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

            var fileLog = Configuration.GetValue<string>("FileLogLocation");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(fileLog, fileSizeLimitBytes: 31457280)
                .WriteTo.Console()
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
