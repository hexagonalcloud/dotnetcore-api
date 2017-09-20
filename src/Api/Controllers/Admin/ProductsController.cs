using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Data;
using Api.Filters;
using Api.Models;
using Api.Parameters;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Admin
{
    [Authorize]
    [Route("api/admin/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductData _data;
        private readonly IUrlService _urlService;

        public ProductsController(IProductData data, IUrlService urlService)
        {
            _data = data;
            _urlService = urlService;
        }

        // [EntityTagFilter]
        [ProducesResponseType(304)]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [HttpGet(Name = "GetAdminProducts")]
        public async Task<IActionResult> Get([FromQuery] ProductQueryParameters queryParameters)
        {
            var pagedList = await _data.Get(queryParameters);
            var linkHeader = _urlService.GetLinkHeader("GetAdminProducts", pagedList);
            Response.Headers.Add("Link", linkHeader);
            return Ok(pagedList);
        }

        // [EntityTagFilter]
        [ProducesResponseType(typeof(AdminProduct), 200)]
        [ProducesResponseType(304)]
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _data.GetAdminProductById(id));
        }

        [ProducesResponseType(201)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AdminProduct product)
        {
            if (ModelState.IsValid)
            {
                var result = await _data.Create(product);
            }

            return new CreatedResult(string.Empty, null);
        }
    }
}
