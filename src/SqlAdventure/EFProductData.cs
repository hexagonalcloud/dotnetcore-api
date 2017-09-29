using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using Core;
using Core.Entities;
using Core.Parameters;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SqlAdventure.Database;
using SqlAdventure.Services;
using Product = Core.Entities.Product;

namespace SqlAdventure
{
    // ReSharper disable once InconsistentNaming
    public class EFProductData : IProductData
    {
        private readonly IMapper _mapper;
        private readonly ISqlAdventureContext _dbContext;
        private readonly ILogger _logger;
        private readonly ISqlClauseService _sqlClauseService;

        public EFProductData(IMapper mapper, ISqlAdventureContext dbContext, ILogger logger, ISqlClauseService sqlClauseService)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
            _sqlClauseService = sqlClauseService;
        }

        public async Task<IPagedList<Product>> Get(ProductQueryParameters queryParameters)
        {
            // TODO: filter products depending on admin or not (sell start date? discontinued?)
            var offset = (queryParameters.PageNumber - 1) * queryParameters.PageSize;
            int totalCount = 0;

            /* Looks like we will need to load all the data to get the total count here... TODO: find out if there is way to do the same as in the dapper implementation since this will be an issue with large result sets. Running a separate count query will not work with EF it seems due to the current limitations (https://docs.microsoft.com/en-us/ef/core/querying/raw-sql), so either use ado.net or dapper...
             TODO: run 2 queries  in parallel?
            var totalCountQuery = "SELECT COUNT(DISTINCT ProductID) from [SalesLT].Product";*/

            var dbQuery = _dbContext.Product
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductModel)
                .OrderBy(p => p.Name).AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParameters.SearchQuery))
            {
                dbQuery = dbQuery.Where(p => p.Name.Contains(queryParameters.SearchQuery));
            }

            if (!string.IsNullOrWhiteSpace(queryParameters.Color))
            {
                var whereClause = _sqlClauseService.CreateWhereClause("Color", queryParameters.Color);
                if (!string.IsNullOrWhiteSpace(whereClause.predicate))
                {
                    dbQuery = dbQuery.Where(whereClause.predicate, whereClause.parameters);
                }
            }

            if (!string.IsNullOrWhiteSpace(queryParameters.OrderBy))
            {
                var orderClause = _sqlClauseService.CreateOrderClause(queryParameters.OrderBy);
                if (!string.IsNullOrWhiteSpace(orderClause))
                {
                    dbQuery = dbQuery.OrderBy(orderClause);
                }
            }

            totalCount = await dbQuery.CountAsync();
            var dbResult = dbQuery.Skip(offset).Take(queryParameters.PageSize);

            var result = _mapper.Map<List<Product>>(dbResult);
            var pagedResult = new PagedList<Product>(result, totalCount, queryParameters);
            return pagedResult;
        }

        public async Task<Product> GetById(Guid id)
        {
            var dbProduct = await _dbContext.Product
                .AsNoTracking()
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductModel)
                .SingleOrDefaultAsync(p => p.Rowguid == id);

            if (dbProduct != null)
            {
                var result = Mapper.Map<Product>(dbProduct);
                return result;
            }

            return null;
        }

        public async Task<AdminProduct> GetAdminProductById(Guid id)
        {
            var dbProduct = await _dbContext.Product
                .AsNoTracking()
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductModel)
                .SingleOrDefaultAsync(p => p.Rowguid == id);

            if (dbProduct != null)
            {
                var result = Mapper.Map<AdminProduct>(dbProduct);
                return result;
            }

            return null;
        }

        public async Task<Guid> Create(CreateProduct product)
        {
            var dbProduct = _mapper.Map<Database.Product>(product);
            dbProduct.Rowguid = Guid.NewGuid();

            _dbContext.Product.Add(dbProduct);
            var result = await _dbContext.SaveChangesAsync();
            if (result == 1)
            {
                return dbProduct.Rowguid;
            }

            return Guid.Empty;
        }

        public async Task<bool> Delete(Guid id)
        {
            var product = await _dbContext.Product
                .SingleOrDefaultAsync(p => p.Rowguid == id);
            if (product == null)
            {
                return false;
            }

            _dbContext.Product.Remove(product);
            var result = await _dbContext.SaveChangesAsync();

            return result == 1;
        }

        public async Task<bool> Update(UpdateProduct product)
        {
            // TODO: verify modified date based on incming ETAG
            // add ETAG constraint header to API PUT and PATCH?
            var updatedProduct = _mapper.Map<Database.Product>(product);

            var dbProduct = await _dbContext.Product.AsNoTracking()
                .SingleOrDefaultAsync(p => p.Rowguid == product.Id);
            if (dbProduct == null)
            {
                return false;
            }

            updatedProduct.ProductId = dbProduct.ProductId;
            _dbContext.Product.Update(dbProduct);

            var result = await _dbContext.SaveChangesAsync();

            return result == 1;
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
    }
}
