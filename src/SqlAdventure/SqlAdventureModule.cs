using Autofac;
using Core;

namespace SqlAdventure
{
    public class SqlAdventureModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationOptions>().AsSelf().SingleInstance();
            builder.RegisterType<ProductData>().As<IProductData>();
        }
    }
}
