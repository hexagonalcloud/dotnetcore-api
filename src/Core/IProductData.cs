using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Parameters;

namespace Core
{
    public interface IProductData
    {
        Task<IPagedList<Product>> Get(ProductQueryParameters queryParameters);

        Task<Product> GetById(Guid id);

        Task<AdminProduct> GetAdminProductById(Guid id);

        Task<Guid> Create(CreateProduct product);

        Task<bool> Delete(Guid id);

        Task<bool> Update(UpdateProduct product);

        Task<IEnumerable<string>> GetModels();

        Task<IEnumerable<string>> GetCategories();

        Task<IEnumerable<string>> GetColors();
    }
}
