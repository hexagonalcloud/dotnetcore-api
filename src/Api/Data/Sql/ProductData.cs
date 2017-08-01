using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<Product>> Get()
        {
            using (IDbConnection db = new SqlConnection(_connectionStrings.SqlAdventure))
            {
                string query = "SELECT TOP (100) [ProductID] ,[Name] FROM [SalesLT].[Product]";
                var response = await db.QueryAsync<Product>(query);
                return (List<Product>)response;
            } 
        }

        public async Task<Product> GetById(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionStrings.SqlAdventure))
            {
                var response = await db.QueryAsync<Product>("SELECT[ProductID] ,[Name] FROM [SalesLT].[Product] WHERE [ProductID]=@ProductID", new { ProductID = id });
                return response.FirstOrDefault();
            }
        }
    }
}