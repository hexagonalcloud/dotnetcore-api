using Api.Data;
using Api.Data.Sql;
using Autofac;

namespace Api
{
	public class AutofacModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ProductData>().As<IProductData>();			
		}
		
	}
}
