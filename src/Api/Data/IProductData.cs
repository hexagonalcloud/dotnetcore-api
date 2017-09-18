using System.Threading.Tasks;
using Api.Models;
using Api.Parameters;

namespace Api.Data
{
    public interface IProductData
    {
        Task<PagedList<Product>> Get(PagingParameters pagingParameters, FilterParameters filterParameters);

        Task<Product> GetById(int id);
    }
}
