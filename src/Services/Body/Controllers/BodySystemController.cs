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
            BodySystem bodySystem = await mediator.Send(new CreateBodySystem(Scaled: false)).ConfigureAwait(false);
            return Ok(bodySystem);
        }

        [HttpGet("scaled")]
        public async Task<IActionResult> GetScaled()
        {
            BodySystem bodySystem = await mediator.Send(new CreateBodySystem(Scaled: true)).ConfigureAwait(false);
            return Ok(bodySystem);
        }
    }
}
