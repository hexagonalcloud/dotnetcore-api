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

        public async Task<PagedList<Product>> Get(ProductQueryParameters queryParameters)
        {
            using (IDbConnection db = new SqlConnection(_connectionStrings.SqlAdventure))
            {
                var offset = (queryParameters.PageNumber - 1) * queryParameters.PageSize;
                int totalCount = 0;
                string whereClause;

                if (!string.IsNullOrWhiteSpace(queryParameters.SearchQuery) && !string.IsNullOrWhiteSpace(queryParameters.Color))
                {
                    whereClause =
                        $"WHERE [Name] LIKE '%{queryParameters.SearchQuery}%' AND [Color] = '{queryParameters.Color}'";
                }
                else if (!string.IsNullOrWhiteSpace(queryParameters.SearchQuery))
                {
                    whereClause = $"WHERE [Name] LIKE '%{queryParameters.SearchQuery}%'";
                }
                else if (!string.IsNullOrWhiteSpace(queryParameters.Color))
                {
                    whereClause = $"WHERE [Color] = '{queryParameters.Color}'";
                }
                else
                {
                    whereClause = string.Empty;
                }

                 var query = $"SELECT *, COUNT(*) OVER () as TotalCount FROM [SalesLT].[Product] {whereClause} ORDER BY [ProductID] OFFSET {offset} ROWS FETCH NEXT {queryParameters.PageSize} ROWS ONLY";

                Func<Product, int, Product> map = (result, count) =>
                {
                    totalCount = count;
                    return result;
                };

               var response = await db.QueryAsync<Product, int, Product>(query, map, splitOn: "TotalCount");
               var pagedResult = new PagedList<Product>(response.ToList(), totalCount,  queryParameters);
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
