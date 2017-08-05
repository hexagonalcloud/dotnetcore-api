using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Api.Controllers.Integration
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    [ProducesResponseType(401)]
	[Authorize]
    [Route("api/integration/[controller]")]
    public class CacheController : Controller
    {
        private readonly IDistributedCache _distributedCache;

        public CacheController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(Product), 200)]
        [HttpGet("{id}")]
        public async Task <IActionResult> Get(string id)
        {
            var result = await GetAsync<Product>(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [ProducesResponseType(200)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Product product)
        {
            await SetAsync(product.ProductID.ToString(), product);
            //TODO: return Created()
            return Ok();
        }

        [ProducesResponseType(200)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _distributedCache.RemoveAsync(id);
            return Ok();
        }

        public async Task SetAsync<T>(string key, T item, int expirationInHours = 0)
        {
            var json = JsonConvert.SerializeObject(item);
            await _distributedCache.SetStringAsync(key, json);
            //await _distributedCache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            //{
            //    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours()
            //});
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var json = await _distributedCache.GetStringAsync(key);
            if (!string.IsNullOrWhiteSpace(json))
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            return default(T);
        }
    }
}
