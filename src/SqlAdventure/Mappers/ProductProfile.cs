using AutoMapper;
using SqlAdventure.Db;

namespace SqlAdventure.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            var productMap = CreateMap<Core.Entities.Product, Product>().ReverseMap();
            productMap.ForMember(ent => ent.Id, opt => opt.MapFrom(db => db.RowGuid)).ReverseMap();

            var efProductMap = CreateMap<Database.Product, Core.Entities.Product>();
            efProductMap.ForMember(ent => ent.Id, opt => opt.MapFrom(db => db.Rowguid));
            efProductMap.ForMember(ent => ent.Category,
                opt => opt.MapFrom(db => db.ProductCategory != null ? db.ProductCategory.Name : string.Empty));
            efProductMap.ForMember(ent => ent.Model,
                opt => opt.MapFrom(db => db.ProductModel != null ? db.ProductModel.Name : string.Empty));

            var adminMap = CreateMap<Core.Entities.AdminProduct, AdminProduct>().ReverseMap();
            adminMap.ForMember(ent => ent.Id, opt => opt.MapFrom(db => db.RowGuid)).ReverseMap();

            var efAdminProductMap = CreateMap<Database.Product, Core.Entities.AdminProduct>();
            efAdminProductMap.ForMember(ent => ent.Id, opt => opt.MapFrom(db => db.Rowguid));
            efAdminProductMap.ForMember(ent => ent.Category,
                opt => opt.MapFrom(db => db.ProductCategory != null ? db.ProductCategory.Name : string.Empty));
            efAdminProductMap.ForMember(ent => ent.Model,
                opt => opt.MapFrom(db => db.ProductModel != null ? db.ProductModel.Name : string.Empty));

            CreateMap<Core.Entities.CreateProduct, CreateProduct>().ReverseMap();

            var updateMap = CreateMap<Core.Entities.UpdateProduct, UpdateProduct>().ReverseMap();
            updateMap.ForMember(ent => ent.Id, opt => opt.MapFrom(db => db.RowGuid)).ReverseMap();
        }
    }
}
