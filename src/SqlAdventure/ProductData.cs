using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AutoMapper;
using Core;
using Core.Parameters;
using Dapper;
using Dapper.Contrib.Extensions;

using Entities = Core.Entities;

namespace SqlAdventure
{
    public class ProductData : IProductData
    {
        private readonly ConfigurationOptions _configurationOptions;
        private readonly IMapper _mapper;

        public ProductData(ConfigurationOptions options, IMapper mapper)
        {
            _configurationOptions = options;
            _mapper = mapper;
        }

        public async Task<Entities.IPagedList<Entities.Product>> Get(ProductQueryParameters queryParameters)
        {
            // TODO: filter products depending on admin or not
            var offset = (queryParameters.PageNumber - 1) * queryParameters.PageSize;
            int totalCount = 0;

            var createQuery = CreateGetProductsQuery(queryParameters, offset);

            Func<Db.Product, int, Db.Product> map = (result, count) =>
            {
                totalCount = count;
                return result;
            };

            using (IDbConnection db = new SqlConnection(_configurationOptions.SqlConnectionString))
            {
                var response = await db.QueryAsync<Db.Product, int, Db.Product>(createQuery.query, map, splitOn: "TotalCount", param: createQuery.parameters);

                var result = _mapper.Map<List<Entities.Product>>(response);
                var pagedResult = new Entities.PagedList<Entities.Product>(result, totalCount, queryParameters);
                return pagedResult;
            }
        }

        public async Task<Entities.Product> GetById(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_configurationOptions.SqlConnectionString))
            {
                var response = await db.GetAsync<Db.Product>(id);
                var result = _mapper.Map<Entities.Product>(response);
                return result;
            }
        }

        public async Task<Entities.AdminProduct> GetAdminProductById(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_configurationOptions.SqlConnectionString))
            {
                var response = await db.GetAsync<Db.AdminProduct>(id);
                var result = _mapper.Map<Entities.AdminProduct>(response);
                return result;
            }
        }

        public async Task<Guid> Create(Entities.CreateProduct product)
        {
            var dbProduct = _mapper.Map<Db.CreateProduct>(product);
            dbProduct.RowGuid = Guid.NewGuid();

            using (IDbConnection db = new SqlConnection(_configurationOptions.SqlConnectionString))
            {
                await db.InsertAsync(dbProduct);
            }

            return dbProduct.RowGuid;
        }

        public async Task<bool> Delete(Guid id)
        {
            using (IDbConnection db = new SqlConnection(_configurationOptions.SqlConnectionString))
            {
                var result = await db.DeleteAsync(new Db.AdminProduct { RowGuid = id });
                return result;
            }
        }

        public async Task<bool> Update(Entities.UpdateProduct product)
        {
            using (IDbConnection db = new SqlConnection(_configurationOptions.SqlConnectionString))
            {
                // TODO: verify modified date based on incming ETAG
                // add ETAG constraint header to API PUT and PATCH?
                var dbProduct = _mapper.Map<Db.UpdateProduct>(product);
                var result = await db.UpdateAsync(dbProduct);
                return result;
            }
        }

        public Task<IEnumerable<string>> GetModels()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetCategories()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetColors()
        {
            throw new NotImplementedException();
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
