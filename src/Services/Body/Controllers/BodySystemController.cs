using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sas.Body.Service.Models.Domain.BodySystems;
using Sas.Body.Service.Models.Queries;

namespace Sas.Body.Service.Controllers
{
    [ApiController]
    [Route("body-system")]
    public class BodySystemController(IMediator mediator) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            CreateBodySystem request = new(SkipStaticPoints: false);
            BodySystem bodySystem = await mediator.Send(request).ConfigureAwait(false);
            return Ok(bodySystem);
        }

        [HttpGet("{atTime}")]
        public async Task<IActionResult> GetAtTime(double atTime)
        {
            CreateBodySystem request = new(SkipStaticPoints: true);
            BodySystem bodySystem = await mediator.Send(request).ConfigureAwait(false);
            bodySystem.Move(atTime);
            return Ok(bodySystem);
        }
    }
}
