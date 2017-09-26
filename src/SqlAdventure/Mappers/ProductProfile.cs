using AutoMapper;
using SqlAdventure.Db;
using Database = SqlAdventure.Database;

namespace SqlAdventure.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            var productMap = CreateMap<Core.Entities.Product, Product>().ReverseMap();
            productMap.ForMember(source => source.Id, opt => opt.MapFrom(dest => dest.RowGuid)).ReverseMap();

            var efProductMap = CreateMap<Core.Entities.Product, Database.Product>().ReverseMap();
            efProductMap.ForMember(source => source.Id, opt => opt.MapFrom(dest => dest.Rowguid)).ReverseMap();

            var adminMap = CreateMap<Core.Entities.AdminProduct, AdminProduct>().ReverseMap();
            adminMap.ForMember(source => source.Id, opt => opt.MapFrom(dest => dest.RowGuid)).ReverseMap();

            CreateMap<Core.Entities.CreateProduct, CreateProduct>().ReverseMap();

            var updateMap = CreateMap<Core.Entities.UpdateProduct, UpdateProduct>().ReverseMap();
            updateMap.ForMember(source => source.Id, opt => opt.MapFrom(dest => dest.RowGuid)).ReverseMap();
        }
    }
}
