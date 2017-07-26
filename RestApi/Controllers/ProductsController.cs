using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        public ProductsController()
        {
            
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(
                new Product[] { new Product() { Name = "Product One" }, new Product() { Name = "Product Two" } });
        }

        public class Product
        {
            public string Name { get; set; }
        }
    }
}