using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Data
{
    public interface IProductData
    {
        Task<IEnumerable<Product>> Get();
        Task<Product> GetById(int id);
    }
}