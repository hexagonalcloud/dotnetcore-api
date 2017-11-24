using Autofac;
using Core;
using Core.Data;
using SqlAdventure.Database;
using SqlAdventure.Services;

namespace SqlAdventure
{
    public class SqlAdventureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationOptions>().AsSelf().SingleInstance();
            builder.RegisterType<ProductRepository>().As<IProductRepository>();
            builder.RegisterType<SqlAdventureContext>().As<ISqlAdventureContext>();
            builder.RegisterType<SqlClauseService>().As<ISqlClauseService>();
        }
    }
}
