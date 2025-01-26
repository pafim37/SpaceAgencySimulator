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
        [HttpGet("{G}")]
        public async Task<IActionResult> Get(double G)
        {
            BodySystem bodySystem = await mediator.Send(new CreateBodySystem(G)).ConfigureAwait(false);
            return Ok(bodySystem);
        }
    }
}
