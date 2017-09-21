using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Data;
using Api.Models;
using Api.Parameters;
using Api.Results;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        [ProducesResponseType(404)]
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _data.GetAdminProductById(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return Ok(result);
        }

        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(ModelStateDictionary), 422)]
        [ProducesResponseType(typeof(CreateProduct), 201)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProduct product)
        {
            if (product == null)
            {
                return new BadRequestResult();
            }

            if (ModelState.IsValid)
            {
               var result = await _data.Create(product); // todo: check if exists
                return CreatedAtRoute("GetAdminProducts", new { id = product.RowGuid }, product); // TODO: do we need to return the product here? Yes, but not this one, we want to return one with an id....
            }

            return new UnprocessableEntityObjectResult(ModelState);
        }

        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _data.Delete(id);
            if (result)
            {
                return Ok();
            }

            return NotFound();
        }

        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(ModelStateDictionary), 422)]
        [ProducesResponseType(204)]
        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> Put(Guid id, [FromBody] UpdateProduct product)
        {
            if (product == null)
            {
                return new BadRequestResult();
            }

            product.RowGuid = id; // TODO: also allow requests with id's in the request object? and allow creation? (will always create a new product)

            if (ModelState.IsValid)
            {
                await _data.Update(product);
                return new NoContentResult();
            }

            return new UnprocessableEntityObjectResult(ModelState);
        }

        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(ModelStateDictionary), 422)]
        [ProducesResponseType(204)]
        [Route("{id}")]
        [HttpPatch]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<UpdateProduct> patchProduct)
        {
            if (patchProduct == null)
            {
                return new BadRequestResult();
            }

            var adminProduct = await _data.GetAdminProductById(id);
            if (adminProduct == null)
            {
                return new NotFoundResult();
            }

            var updateProduct = new UpdateProduct(adminProduct);
            patchProduct.ApplyTo(updateProduct, ModelState);
            TryValidateModel(updateProduct);

            if (ModelState.IsValid)
            {
                await _data.Update(updateProduct);
                return new NoContentResult();
            }

            return new UnprocessableEntityObjectResult(ModelState);
        }
    }
}
