using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Public
{
    [Route("api/public/[controller]")]
    public class ProductsController : Controller
    {
        private const int MaxPageSize = 20;
        private readonly IProductData _data;
        private readonly IUrlHelper _urlHelper;

        public ProductsController(IProductData data, IUrlHelper urlHelper)
        {
            _data = data;
            _urlHelper = urlHelper;
        }

        [ResponseCache(CacheProfileName = "Default")]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult> Get([FromQuery]int? pageNumber, [FromQuery]int? pageSize)
        {
            var page = pageNumber.HasValue && pageNumber.Value > 0 ? pageNumber.Value : 1;
            var size = pageSize.HasValue && pageSize.Value < MaxPageSize ? pageSize.Value : MaxPageSize;

            var pagedList = await _data.Get(page, size);
            var linkHeader = CreateLinkHeader(pagedList);

            // TODO: possible additional headers?
            // max-pagesize?
            // default pagesize?
            // only include the pagesize if the pagesize is not the default pagesize, or if the pagezsize was specified? Probably.
            // add sorting once that is implemented to the links, same as page size, only add to the links if it there is a param and it is valid.

            Response.Headers.Add("Link", string.Join(", ", linkHeader.ToArray()));
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

        private IEnumerable<string> CreateLinkHeader(PagedList<Product> pagedList)
        {
            // for now based on https://developer.github.com/v3/#pagination
            // TODO: do not add page size if the query param is null
            var linkHeaders = new List<string>();

            if (pagedList.HasNext)
            {
                var linkHeader = "<" + _urlHelper.Link("GetProducts", new { pageNumber = pagedList.CurrentPage + 1, pageSize = pagedList.PageSize }).ToLowerInvariant() + ">; rel=next";
                linkHeaders.Add(linkHeader);
            }

            if (!pagedList.IsLastPage)
            {
                var linkHeader = "<" + _urlHelper.Link("GetProducts", new { pageNumber = pagedList.TotalPages, pageSize = pagedList.PageSize }).ToLowerInvariant() + ">; rel=last";
                linkHeaders.Add(linkHeader);
            }

            if (!pagedList.IsFirstPage)
            {
                var linkHeader = "<" + _urlHelper.Link("GetProducts", new { pageNumber = 1, pageSize = pagedList.PageSize }).ToLowerInvariant() + ">; rel=first";
                linkHeaders.Add(linkHeader);
            }

            if (pagedList.HasPrevious)
            {
                var linkHeader = "<" + _urlHelper.Link("GetProducts", new { pageNumber = pagedList.CurrentPage - 1, pageSize = pagedList.PageSize }).ToLowerInvariant() + ">; rel=prev";
                linkHeaders.Add(linkHeader);
            }

            return linkHeaders;
        }
    }
}
