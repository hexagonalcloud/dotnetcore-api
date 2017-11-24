using AutoMapper;

namespace Api.Mappers
{
    public class ProductProfile: Profile
    {
        public ProductProfile()
        {
            CreateMap<Core.Entities.Product, Api.Models.Admin.Product>().ReverseMap();
            CreateMap<Core.Entities.Product, Api.Models.Admin.Product>();
        }
    }
}
