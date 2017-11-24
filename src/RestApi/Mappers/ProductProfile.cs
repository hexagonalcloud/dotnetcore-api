using AutoMapper;
using RestApi.Models.Admin;

namespace RestApi.Mappers
{
    public class ProductProfile: Profile
    {
        public ProductProfile()
        {
            CreateMap<Core.Entities.Product, Product>().ReverseMap();
            CreateMap<Core.Entities.Product, Product>();
        }
    }
}
