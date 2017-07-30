using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Api.Models;
using Dapper;
using Microsoft.Extensions.Options;

namespace Api.Data.Sql
{
    public class ProductData : IProductData
    {
        private readonly ConnectionStrings _connectionStrings;

        public ProductData(IOptions<ConnectionStrings> options)
        {
            _connectionStrings = options.Value;
        }

        public IEnumerable<Product> Get()
        {
            using (IDbConnection db = new SqlConnection(_connectionStrings.SqlAdventure))
            {
                string query = "SELECT TOP (100) [ProductID] ,[Name] FROM [SalesLT].[Product]";
                var products = (List<Product>)db.Query<Product>(query);
                return products;
            } 
        }

        public Product GetById(string id)
        {
            using (IDbConnection db = new SqlConnection(_connectionStrings.SqlAdventure))
            {
                return db.Query<Product>("SELECT[ProductID] ,[Name] FROM [SalesLT].[Product] WHERE [ProductID]=@ProductID", new { ProductID = id }).SingleOrDefault();
            }
        }
    }
}