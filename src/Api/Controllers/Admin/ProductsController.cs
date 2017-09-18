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

        [EntityTagFilter]
        [ProducesResponseType(304)]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [HttpGet(Name = "GetAdminProducts")]
        public async Task<IActionResult> Get([FromQuery] PagingParameters pagingParameters, [FromQuery] FilterParameters filterParameters)
        {
            var pagedList = await _data.Get(pagingParameters, filterParameters);
            var linkHeader = _urlService.CreateLinkHeader("GetAdminProducts", pagedList);

            // TODO: possible additional headers?
            // max-pagesize?
            // default pagesize?
            // only include the pagesize if the pagesize is not the default pagesize, or if the pagezsize was specified? Probably.
            // add sorting once that is implemented to the links, same as page size, only add to the links if it there is a param and it is valid.

            Response.Headers.Add("Link", linkHeader);
            return Ok(pagedList);
        }

        [EntityTagFilter]
        [ProducesResponseType(typeof(Product), 200)]
        [ProducesResponseType(304)]
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _data.GetById(id));
        }
    }
}
