using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Data;
using Api.Models;
using Api.Parameters;
using Api.Services;
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

        [ResponseCache(CacheProfileName = "Default", VaryByQueryKeys = new[] { "PageNumber", "PageSize", "SearchQuery", "Color" })]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult> Get([FromQuery] ProductQueryParameters queryParameters)
        {
            var pagedList = await _data.Get(queryParameters);
            var linkHeader = _urlService.GetLinkHeader("GetProducts", pagedList);
            Response.Headers.Add("Link", linkHeader);
            return Ok(pagedList);
        }

        [ResponseCache(CacheProfileName = "Default")]
        [ProducesResponseType(typeof(Product), 200)]
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _data.GetById(id));
        }
    }
}
