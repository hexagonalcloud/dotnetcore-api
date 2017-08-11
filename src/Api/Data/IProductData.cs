using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Data
{
    public interface IProductData
    {
        Task<PagedList<Product>> Get(int pageNumber, int pageSize);

        Task<Product> GetById(int id);
    }
}
