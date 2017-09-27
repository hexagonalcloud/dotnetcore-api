using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core;
using Entities = Core.Entities;
using Core.Parameters;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SqlAdventure.Database;

namespace SqlAdventure
{
    public class EFroductData : IProductData
    {
        private readonly ConfigurationOptions _configurationOptions;
        private readonly IMapper _mapper;
        private readonly ISqlAdventureContext _dbContext;
        private readonly ILogger _logger;

        public EFroductData(ConfigurationOptions options, IMapper mapper, ISqlAdventureContext dbContext, ILogger logger)
        {
            _configurationOptions = options;
            _mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Entities.IPagedList<Entities.Product>> Get(ProductQueryParameters queryParameters)
        {
            // TODO: filter products depending on admin or not (sell start date? discontinued?)
            var offset = (queryParameters.PageNumber - 1) * queryParameters.PageSize;
            int totalCount = 0;

            /* Looks like we will need to load all the data to get the total count here... TODO: find out if there is way to do the same as in the dapper implementation since this will be an issue with large result sets. Running a separate count query will not work with EF it seems due to the current limitations (https://docs.microsoft.com/en-us/ef/core/querying/raw-sql), so either use ado.net or dapper...
             TODO: run 2 queries  in parallel?
            var totalCountQuery = "SELECT COUNT(DISTINCT ProductID) from [SalesLT].Product";*/
            
            var dbQuery =  _dbContext.Product
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductModel) 
                .OrderBy(p => p.Name).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParameters.SearchQuery))
            {
                dbQuery = dbQuery.Where(p => p.Name.Contains(queryParameters.SearchQuery));
            }

            if (!string.IsNullOrWhiteSpace(queryParameters.Color))
            {
                dbQuery = dbQuery.Where(p => p.Color == queryParameters.Color.Trim());
            }

            //var orderByClause = !string.IsNullOrWhiteSpace(queryParameters.OrderBy) ? $"ORDER BY {queryParameters.OrderBy}" : "ORDER BY Name";

            totalCount = await dbQuery.CountAsync();
            var dbResult = dbQuery.Skip(offset).Take(queryParameters.PageSize);

            // map
            var result = _mapper.Map<List<Entities.Product>>(dbResult);
            var pagedResult = new Entities.PagedList<Entities.Product>(result, totalCount, queryParameters);
            return pagedResult;

            //var createQuery = CreateGetProductsQuery(queryParameters, offset);

            //Func<Db.Product, int, Db.Product> map = (result, count) =>
            //{
            //    totalCount = count;
            //    return result;
            //};

            //using (IDbConnection db = new SqlConnection(_configurationOptions.SqlConnectionString))
            //{
            //    var response = await db.QueryAsync<Db.Product, int, Db.Product>(createQuery.query, map, splitOn: "TotalCount", param: createQuery.parameters);

            //    var result = _mapper.Map<List<Entities.Product>>(response);
            //    //var pagedResult = new Entities.PagedList<Entities.Product>(result, totalCount, queryParameters);
            //    return pagedResult;
            //}
        }

        public async Task<Entities.Product> GetById(Guid id)
        {
            try
            {
                var dbProduct = await _dbContext.Product
                    .Include(p => p.ProductCategory)
                    .Include(p => p.ProductModel)
                    .SingleOrDefaultAsync(p => p.Rowguid == id);    

                if (dbProduct != null)
                {
                    var result = Mapper.Map<Entities.Product>(dbProduct);
                    return result;
                }

                return null;
            }
            catch (SqlException exception)
            {
                // we seem to be loosing the context when we get an exception here
                _logger.Error(exception, string.Empty);
                throw;
            } 
        }

        public async Task<Entities.AdminProduct> GetAdminProductById(Guid id)
        {
            try
            {
                var dbProduct = await _dbContext.Product
                    .Include(p => p.ProductCategory)
                    .Include(p => p.ProductModel)
                    .SingleOrDefaultAsync(p => p.Rowguid == id);

                if (dbProduct != null)
                {
                    var result = Mapper.Map<Entities.AdminProduct>(dbProduct);
                    return result;
                }

                return null;
            }
            catch (SqlException exception)
            {
                // we seem to be loosing the context when we get an exception here
                _logger.Error(exception, string.Empty);
                throw;
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

        public async Task<IEnumerable<string>> GetModels()
        {
            var dbQuery = _dbContext.ProductModel
                .OrderBy(p => p.Name)
                .Select(p => p.Name);

            return await dbQuery.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetCategories()
        {
            var dbQuery = _dbContext.ProductCategory
                .Where(p => p.ParentProductCategoryId == null)
                .OrderBy(p => p.Name)
                .Select(p => p.Name);

            return await dbQuery.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetColors()
        {
            var dbQuery = _dbContext.Product
                .Select(p => p.Color)
                .Where(p => p != null)
                .Distinct()
                .OrderBy(p => p);

            return await dbQuery.ToListAsync();
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
