using Api.Data;
using Api.Data.Sql;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Api
{
    public class AutofacModule : Module
    {
        private readonly IHostingEnvironment _environment;
        private readonly IConfigurationRoot _configurationRoot;

        public AutofacModule(IHostingEnvironment environment, IConfigurationRoot configurationRoot)
        {
            _environment = environment;
            _configurationRoot = configurationRoot;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProductData>().As<IProductData>();

            var fileLog = _configurationRoot.GetValue<string>("FileLogLocation");

            if (_environment.IsDevelopment())
            {
                builder.Register<ILogger>(_ => new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.File(fileLog, fileSizeLimitBytes: 31457280)
                    .WriteTo.Console()
                    .CreateLogger()).SingleInstance();
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(fileLog))
                {
                    builder.Register<ILogger>(_ => new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.File(fileLog, fileSizeLimitBytes: 31457280)
                        .CreateLogger()).SingleInstance();
                }
            }
        }
    }
}
