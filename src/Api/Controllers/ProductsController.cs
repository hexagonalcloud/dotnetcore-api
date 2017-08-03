using System.Collections.Generic;
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

        public ProductsController(IProductData data)
        {
            _data = data;
        }

        [ResponseCache(CacheProfileName = "Default")]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _data.Get());
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