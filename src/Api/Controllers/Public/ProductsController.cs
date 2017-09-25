using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        [ResponseCache(CacheProfileName = "Default", VaryByQueryKeys = new[] { "PageNumber", "PageSize", "SearchQuery", "Color", "OrderBy" })]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult> Get([FromQuery] ProductQueryParameters queryParameters)
        {
            var pagedList = await _data.Get(queryParameters);
            var linkHeader = _urlService.GetLinkHeader("GetProducts", pagedList);
            Response.Headers.Add("Link", linkHeader);
            return Ok(pagedList);
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
    }
}
