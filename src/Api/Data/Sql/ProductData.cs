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
            // TODO: filter products depending on admin or not
            var offset = (queryParameters.PageNumber - 1) * queryParameters.PageSize;
            int totalCount = 0;

            var createQuery = CreateGetProductsQuery(queryParameters, offset);

            Func<Product, int, Product> map = (result, count) =>
            {
                totalCount = count;
                return result;
            };

            using (IDbConnection db = new SqlConnection(_connectionStrings.SqlAdventure))
            {
               var response = await db.QueryAsync<Product, int, Product>(createQuery.query, map, splitOn: "TotalCount", param: createQuery.parameters);
               var pagedResult = new PagedList<Product>(response.ToList(), totalCount,  queryParameters);
                return pagedResult;
            }
        }

        public async Task<Product> GetById(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionStrings.SqlAdventure))
            {
                var response = await db.GetAsync<Product>(id);
                return response;
            }
        }

        public async Task<AdminProduct> GetAdminProductById(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_connectionStrings.SqlAdventure))
            {
                var response = await db.GetAsync<AdminProduct>(id);
                return response;
            }
        }

        public async Task<Guid> Create(CreateProduct product)
        {
            if (product.RowGuid == Guid.Empty)
            {
                product.RowGuid = Guid.NewGuid();
            }

            using (IDbConnection db = new SqlConnection(_connectionStrings.SqlAdventure))
            {
                try
                {
                    var createResult = await db.InsertAsync(product);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return product.RowGuid;
        }

        private static(string query, DynamicParameters parameters) CreateGetProductsQuery(ProductQueryParameters queryParameters, int offset)
        {
            string whereClause;

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(queryParameters.SearchQuery) && !string.IsNullOrWhiteSpace(queryParameters.Color))
            {
                whereClause =
                    "WHERE [Name] LIKE @Name AND [Color] = @Color";
                parameters.Add("@Name", "%" + queryParameters.SearchQuery + "%");
                parameters.Add("@Color", queryParameters.Color);
            }
            else if (!string.IsNullOrWhiteSpace(queryParameters.SearchQuery))
            {
                whereClause = $"WHERE [Name] LIKE @Name";
                parameters.Add("@Name", "%" + queryParameters.SearchQuery + "%");
            }
            else if (!string.IsNullOrWhiteSpace(queryParameters.Color))
            {
                whereClause = $"WHERE [Color] = @Color";
                parameters.Add("@Color", queryParameters.Color);
            }
            else
            {
                whereClause = string.Empty;
            }

            var orderByClause = !string.IsNullOrWhiteSpace(queryParameters.OrderBy) ? $"ORDER BY {queryParameters.OrderBy}" : "ORDER BY Name";

            var query = $"SELECT *, COUNT(*) OVER () as TotalCount FROM [SalesLT].[Product] {whereClause} {orderByClause} OFFSET {offset} ROWS FETCH NEXT {queryParameters.PageSize} ROWS ONLY";

            return (query, parameters);
        }
    }
}
