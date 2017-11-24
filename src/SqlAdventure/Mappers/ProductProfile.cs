using AutoMapper;

namespace SqlAdventure.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            var productMap = CreateMap<Database.Product, Core.Entities.Product>();
            productMap.ForMember(ent => ent.Id, opt => opt.MapFrom(db => db.Rowguid));
            productMap.ForMember(ent => ent.Category, opt => opt.MapFrom(db => db.ProductCategory != null ? db.ProductCategory.Name : string.Empty));
            productMap.ForMember(ent => ent.Model, opt => opt.MapFrom(db => db.ProductModel != null ? db.ProductModel.Name : string.Empty));

            CreateMap<Core.Entities.CreateProduct, Database.Product>();

            var updateMap = CreateMap<Core.Entities.UpdateProduct, Database.Product>();
            updateMap.ForMember(db => db.Rowguid, opt => opt.MapFrom(ent => ent.Id));
        }
    }
}
