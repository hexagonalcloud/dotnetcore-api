using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Api.Parameters;
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

        public async Task<PagedList<Product>> Get(PagingParameters pagingParameters, FilterParameters filterParameters)
        {
            using (IDbConnection db = new SqlConnection(_connectionStrings.SqlAdventure))
            {
                var offset = (pagingParameters.PageNumber - 1) * pagingParameters.PageSize;
                int totalCount = 0;
                string query;

                if (string.IsNullOrWhiteSpace(filterParameters.Color))
                {
                    query = $"SELECT *, COUNT(*) OVER () as TotalCount FROM [SalesLT].[Product] ORDER BY [ProductID] OFFSET {offset} ROWS FETCH NEXT {pagingParameters.PageSize} ROWS ONLY";
                }
                else
                {
                    query = $"SELECT *, COUNT(*) OVER () as TotalCount FROM [SalesLT].[Product] WHERE [Color] = '{filterParameters.Color}' ORDER BY [ProductID] OFFSET {offset} ROWS FETCH NEXT {pagingParameters.PageSize} ROWS ONLY";
                }

                Func<Product, int, Product> map = (result, count) =>
                {
                    totalCount = count;
                    return result;
                };

               var response = await db.QueryAsync<Product, int, Product>(query, map, splitOn: "TotalCount");
               var pagedResult = new PagedList<Product>(response.ToList(), totalCount, pagingParameters, filterParameters);
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
