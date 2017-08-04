using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductData _data;
        private const int MaxPageSize = 20;

        public ProductsController(IProductData data)
        {
            _data = data;
        }

        [ResponseCache(CacheProfileName = "Default")]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int? pageNumber = 1, [FromQuery]int? pageSize = 10)
        {
            if (pageSize > MaxPageSize)
            {
                pageSize = MaxPageSize;
            }

            //X - Pagination - Count: 100
            //X - Pagination - Page: 5
            //X - Pagination - Limit: 20

            var pagedList = await _data.Get(pageNumber.GetValueOrDefault(), pageSize.GetValueOrDefault());

            // we need some additional data here

            Response.Headers.Add("X-Pagination-Count", $"{pagedList.TotalPages}");
            Response.Headers.Add("X-Pagination-Page", $"{pagedList.CurrentPage}");

            return Ok(pagedList);
        }

        [EntityTagFilter]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(304)]
        [Route("{id}")]        
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _data.GetById(id));
        }
    }
}