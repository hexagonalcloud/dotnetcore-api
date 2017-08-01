using System;
using System.Collections.Generic;
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
        public IActionResult Get()
        {
            return Ok(_data.Get());
        }

        [ProducesResponseType(typeof(Product), 200)]
        [Route("{id}")]        
        [HttpGet]
        public ObjectResult Get(int id)
        {
            return Ok(_data.GetById(id));
        }
    }
}