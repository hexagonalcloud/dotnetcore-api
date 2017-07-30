using System;
using System.Collections.Generic;
using Api.Models;

namespace Api.Data.Sql
{
    public class ProductData : IProductData
    {
        public IEnumerable<Product> Get()
        {
            throw new NotImplementedException();
        }

        public Product GetById(string id)
        {
            throw new NotImplementedException();
        }
    }
}