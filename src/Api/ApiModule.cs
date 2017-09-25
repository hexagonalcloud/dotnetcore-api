using Api.Services;
using Autofac;
using AutofacSerilogIntegration;

namespace Api
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterLogger();
            builder.RegisterType<UrlService>().As<IUrlService>().SingleInstance();
        }
    }
}
