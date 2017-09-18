using Api.Data;
using Api.Data.Sql;
using Api.Services;
using Autofac;
using AutofacSerilogIntegration;
using Microsoft.Extensions.Configuration;

namespace Api
{
    public class AutofacModule : Module
    {
        private readonly IConfiguration _configurationRoot;

        public AutofacModule(IConfiguration configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterLogger();
            builder.RegisterType<UrlService>().As<IUrlService>().SingleInstance();
            builder.RegisterType<ProductData>().As<IProductData>();
        }
    }
}
