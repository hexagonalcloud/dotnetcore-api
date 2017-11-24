using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Parameters;

namespace Core.Data
{
    public interface IProductRepository
    {
        Task<IPagedList<Product>> Get(ProductQueryParameters queryParameters);

        Task<Product> GetById(Guid id);

        Task<Guid> Add(CreateProduct product);

        Task<bool> Remove(Guid id);

        Task<bool> Update(UpdateProduct product);

        Task<IEnumerable<string>> GetModels();

        Task<IEnumerable<string>> GetCategories();

        Task<IEnumerable<string>> GetColors();
    }
}
