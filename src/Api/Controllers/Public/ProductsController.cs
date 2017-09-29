using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api;
using Api.Services;
using Core;
using Core.Entities;
using Core.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Public
{
    [Route("api/public/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductData _data;
        private readonly IUrlService _urlService;

        public ProductsController(IProductData data, IUrlService urlService)
        {
            _data = data;
            _urlService = urlService;
        }

        [ProducesResponseType(304)]
        [ResponseCache(CacheProfileName = "Default", VaryByQueryKeys = new[] { "PageNumber", "PageSize", "SearchQuery", "Color", "OrderBy", "Fields" })]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult> Get([FromQuery] ProductQueryParameters queryParameters)
        {
            var products = await _data.Get(queryParameters);

            var linkHeader = _urlService.GetLinkHeader("GetProducts", products);
            Response.Headers.Add("Link", linkHeader);

            if (!string.IsNullOrWhiteSpace(queryParameters.Fields))
            {
                return Ok(products.SelectFields(queryParameters.Fields));
            }

            return Ok(products);
        }

        [ProducesResponseType(304)]
        [ProducesResponseType(404)]
        [ResponseCache(CacheProfileName = "Default")]
        [ProducesResponseType(typeof(Product), 200)]
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _data.GetById(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return Ok(result);
        }

        [Route("models")]
        [ProducesResponseType(304)]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetModels()
        {
            var models = await _data.GetModels();
            return Ok(models);
        }

        [Route("colors")]
        [ProducesResponseType(304)]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetColors()
        {
            var models = await _data.GetColors();
            return Ok(models);
        }

        [Route("categories")]
        [ProducesResponseType(304)]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var models = await _data.GetCategories();
            return Ok(models);
        }
    }
}
