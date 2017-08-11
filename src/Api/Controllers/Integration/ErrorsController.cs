using System;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Integration
{
    [Produces("application/json")]
    [Authorize]
    [Route("api/integration/[controller]")]
    public class ErrorsController : Controller
    {
        [ProducesResponseType(500)] // TODO: add 500 to the list of possible swagger responses for all requests?
        [ProducesResponseType(typeof(Product), 200)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            throw new ArgumentOutOfRangeException("thingy");
            return await Task.FromResult<IActionResult>(new OkObjectResult(new Product()));
        }
    }
}
