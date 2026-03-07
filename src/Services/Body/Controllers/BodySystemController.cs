using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sas.Body.Service.Models.Domain.BodySystems;
using Sas.Body.Service.Models.Queries;

namespace Sas.Body.Service.Controllers
{
    [ApiController]
    [Route("body-system")]
    public class BodySystemController(IMediator mediator, IMemoryCache cache) : Controller
    {
        private const string BodySystemCacheKey = "BodySystem";

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            CreateBodySystem request = new(SkipStaticPoints: false);
            BodySystem bodySystem = await mediator.Send(request).ConfigureAwait(false);
            cache.Set(BodySystemCacheKey, bodySystem);
            return Ok(bodySystem);
        }

        [HttpGet("{atTime}")]
        public async Task<IActionResult> GetAtTime(double atTime)
        {
            if (!cache.TryGetValue(BodySystemCacheKey, out BodySystem? bodySystem) || bodySystem == null)
            {
                CreateBodySystem request = new(SkipStaticPoints: true);
                bodySystem = await mediator.Send(request).ConfigureAwait(false);
            }
            bodySystem.Move(atTime);
            return Ok(bodySystem);
        }
    }
}
