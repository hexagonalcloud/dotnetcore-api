using Autofac;
using AutofacSerilogIntegration;
using RestApi.Services;

namespace RestApi
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterLogger();
            builder.RegisterType<UrlService>().As<IUrlService>().InstancePerLifetimeScope();
        }
    }
}
