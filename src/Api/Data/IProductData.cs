using System.Threading.Tasks;
using Api.Models;
using Api.Parameters;

namespace Api.Data
{
    public interface IProductData
    {
        Task<PagedList<Product>> Get(ProductQueryParameters queryParameters);

        Task<Product> GetById(int id);
    }
}
