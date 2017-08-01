using System.Collections.Generic;
using Api.Models;

namespace Api.Data
{
    public interface IProductData
    {
        IEnumerable<Product> Get();
        Product GetById(int id);
    }
}