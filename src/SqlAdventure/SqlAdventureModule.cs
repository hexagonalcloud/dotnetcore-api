using Autofac;
using Core;
using SqlAdventure.Database;
using SqlAdventure.Services;

namespace SqlAdventure
{
    public class SqlAdventureModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationOptions>().AsSelf().SingleInstance();
            builder.RegisterType<EFroductData>().As<IProductData>();
            builder.RegisterType<SqlAdventureContext>().As<ISqlAdventureContext>();
            builder.RegisterType<SqlClauseService>().As<ISqlClauseService>();
        }
    }
}
