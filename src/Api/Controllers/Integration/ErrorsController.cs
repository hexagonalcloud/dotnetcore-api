using System;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Integration
{
    [Authorize]
    [Route("api/integration/[controller]")]
    public class ErrorsController : Controller
    {
        [ProducesResponseType(500)] // TODO: add 500 to the list of possible swagger responses for all requests?
        [ProducesResponseType(typeof(Product), 200)]
        [HttpGet]
        public IActionResult Get()
        {
            throw new ArgumentOutOfRangeException("thingy");
        }
    }
}
