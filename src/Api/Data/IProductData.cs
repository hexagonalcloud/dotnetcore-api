using System;
using System.Threading.Tasks;
using Api.Models;
using Api.Parameters;

namespace Api.Data
{
    public interface IProductData
    {
        Task<PagedList<Product>> Get(ProductQueryParameters queryParameters);

        Task<Product> GetById(Guid id);

        Task<AdminProduct> GetAdminProductById(Guid id);

        Task<Guid> Create(CreateProduct product);

        Task<bool> Delete(Guid id);
    }
}
