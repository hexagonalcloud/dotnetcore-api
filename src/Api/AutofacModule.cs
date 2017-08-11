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

            if (_environment.IsDevelopment())
            {
                builder.Register<ILogger>(_ => new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .CreateLogger()).SingleInstance();
            }
            else
            {
                builder.Register<ILogger>(_ => new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.MongoDBCapped(_configurationRoot.GetConnectionString("MongoDb"), cappedMaxSizeMb: 50, cappedMaxDocuments: 1000)
                    .CreateLogger()).SingleInstance();
            }
        }
    }
}
