using Api.Data;
using Api.Data.Sql;
using Autofac;
using Serilog;

namespace Api
{
	public class AutofacModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ProductData>().As<IProductData>();
			
			builder.Register<ILogger>(_ => new LoggerConfiguration()
				.MinimumLevel.Information()
				.WriteTo.Console()
				.CreateLogger()).SingleInstance();
		}	
	}
}
