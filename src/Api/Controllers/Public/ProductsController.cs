using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api;
using Api.Services;
using Core;
using Core.Data;
using Core.Entities;
using Core.Parameters;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Public
{
    [Route("api/public/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IUrlService _urlService;

        public ProductsController(IProductRepository repository, IUrlService urlService)
        {
            _repository = repository;
            _urlService = urlService;
        }

        /// <summary>
        /// Get a list of products
        /// </summary>
        /// <param name="queryParameters">
        /// The queryparameters
        /// </param>
        /// <response code="304">Not Modified</response>
        [ProducesResponseType(304)]
        [ResponseCache(CacheProfileName = "Default", VaryByQueryKeys = new[] { "PageNumber", "PageSize", "SearchQuery", "Color", "OrderBy", "Fields" })]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult> Get([FromQuery] ProductQueryParameters queryParameters)
        {
            var products = await _repository.Get(queryParameters);

            var linkHeader = _urlService.GetLinkHeader("GetProducts", products);
            Response.Headers.Add("Link", linkHeader);

            if (!string.IsNullOrWhiteSpace(queryParameters.Fields))
            {
                return Ok(products.SelectFields(queryParameters.Fields));
            }

            return Ok(products);
        }

        /// <summary>
        /// Get a single product
        /// </summary>
        /// <param name="id">
        /// A Guid
        /// </param>
        /// <response code="304">Not Modified</response>
        [ProducesResponseType(304)]
        [ProducesResponseType(404)]
        [ResponseCache(CacheProfileName = "Default")]
        [ProducesResponseType(typeof(Product), 200)]
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return new NotFoundResult();
            }

            var result = await _repository.GetById(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return Ok(result);
        }

        /// <summary>
        /// Get a list of product models
        /// </summary>
        /// <response code="304">Not Modified</response>
        [Route("models")]
        [ProducesResponseType(304)]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetModels()
        {
            var models = await _repository.GetModels();
            return Ok(models);
        }

        /// <summary>
        /// Get a list of product colors
        /// </summary>
        /// <response code="304">Not Modified</response>
        [Route("colors")]
        [ProducesResponseType(304)]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetColors()
        {
            var models = await _repository.GetColors();
            return Ok(models);
        }

        /// <summary>
        /// Get a list of product categories
        /// </summary>
        /// <response code="304">Not Modified</response>
        [Route("categories")]
        [ProducesResponseType(304)]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var models = await _repository.GetCategories();
            return Ok(models);
        }
    }
}
