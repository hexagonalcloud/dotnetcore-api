using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Data;
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
        
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _data.Get());
        }

        [ProducesResponseType(typeof(Product), 200)]
        [Route("{id}")]        
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _data.GetById(id));
        }
    }
}