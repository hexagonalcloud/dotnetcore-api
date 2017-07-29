using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    public partial class ProductsController : Controller
    {
        public ProductsController()
        {
            
        }
        
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(
                new Product[] { new Product() { Name = "Product One" }, new Product() { Name = "Product Two" } });
        }

        [ProducesResponseType(typeof(Product), 200)]
        [Route("{id}")]        
        [HttpGet]
        public ObjectResult Get(string id)
        {
            return Ok(
                new Product() { Name = $"Product {id}" });
        }
    }
}