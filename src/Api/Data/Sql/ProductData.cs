using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Dapper;
using Dapper.Contrib.Extensions;
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

        public async Task<PagedList<Product>> Get(int pageNumber, int pageSize)
        {
            using (IDbConnection db = new SqlConnection(_connectionStrings.SqlAdventure))
            {
                var offset = (pageNumber - 1) * pageSize;
                int totalCount = 0;

                string query = $"SELECT *, COUNT(*) OVER () as TotalCount FROM [SalesLT].[Product] ORDER BY [ProductID] OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";

                Func<Product, int, Product> map = (result, count) =>
                {
                    totalCount = count;
                    return result;
                };

               var response = await db.QueryAsync<Product, int, Product>(query, map, splitOn: "TotalCount");
               var pagedResult = new PagedList<Product>(response.ToList(), totalCount, pageNumber, pageSize);
                return pagedResult;
            }
        }

        public async Task<Product> GetById(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionStrings.SqlAdventure))
            {
                var response = await db.GetAsync<Product>(id);
                return response;
            }
        }
    }
}
