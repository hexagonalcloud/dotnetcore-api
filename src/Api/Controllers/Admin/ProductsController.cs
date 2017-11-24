using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Results;
using Api.Services;
using AutoMapper;
using Core.Data;
using Core.Entities;
using Core.Parameters;
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
        private readonly IProductRepository _repository;
        private readonly IUrlService _urlService;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository repository, IUrlService urlService, IMapper mapper)
        {
            _repository = repository;
            _urlService = urlService;
            _mapper = mapper;
        }

        // [WeakEntityTagFilter]
        [ProducesResponseType(304)]
        [ProducesResponseType(typeof(IEnumerable<Product>), 200)]
        [HttpGet(Name = "GetAdminProducts")]
        public async Task<IActionResult> Get([FromQuery] ProductQueryParameters queryParameters)
        {
            var products = await _repository.Get(queryParameters);
            var linkHeader = _urlService.GetLinkHeader("GetAdminProducts", products);
            Response.Headers.Add("Link", linkHeader);

            if (!string.IsNullOrWhiteSpace(queryParameters.Fields))
            {
                return Ok(products.SelectFields(queryParameters.Fields));
            }

            return Ok(products);
        }

        // [WeakEntityTagFilter]
        [ProducesResponseType(typeof(Models.Admin.Product), 200)]
        [ProducesResponseType(304)]
        [ProducesResponseType(404)]
        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _repository.GetById(id);
            if (product == null)
            {
                return new NotFoundResult();
            }

            var result = _mapper.Map<Models.Admin.Product>(product);
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
               var result = await _repository.Add(product);
                return CreatedAtRoute("GetAdminProducts", new { id = result }, product); // TODO: do we need to return the product here? Yes, but not this one, we want to return one with an id....
            }

            return new UnprocessableEntityObjectResult(ModelState);
        }

        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _repository.Remove(id);
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

            product.Id = id; // TODO: also allow requests with id's in the request object? and allow creation? (will always create a new product)

            if (ModelState.IsValid)
            {
                await _repository.Update(product);
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

            var product = await _repository.GetById(id);
            if (product == null)
            {
                return new NotFoundResult();
            }

            var updateProduct = new UpdateProduct(product);
            patchProduct.ApplyTo(updateProduct, ModelState);
            TryValidateModel(updateProduct);

            if (ModelState.IsValid)
            {
                await _repository.Update(updateProduct);
                return new NoContentResult();
            }

            return new UnprocessableEntityObjectResult(ModelState);
        }
    }
}
