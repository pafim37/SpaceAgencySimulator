using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sas.Orbit.Service.Filters;
using Sas.Orbit.Service.Models;
using Sas.Orbit.Service.Models.DataTransferObject;
using Sas.Orbit.Service.Models.Requests;

namespace Sas.Orbit.Service.Controllers
{
    [ApiController]
    [TypeFilter(typeof(OrbitExceptionFilter))]
    [Route("[controller]")]
    public class OrbitController(IMediator mediator) : ControllerBase
    {
        [HttpGet("elements")]
        public async Task<IActionResult> GetOrbitElements([FromBody] OrbitalStateDto orbitalStateDto)
        {
            if (orbitalStateDto.Position is null || orbitalStateDto.Velocity is null)
            {
                return BadRequest("Position and Velocity must be provided.");
            }
            return Ok(await mediator.Send(new GetOrbitRequest(orbitalStateDto.Position, orbitalStateDto.Velocity, orbitalStateDto.GravitationalParameter)).ConfigureAwait(false));
        }

        [HttpGet]
        public async Task<IActionResult> GetTwoBodyProblemOrbit([FromBody] TwoBodyState twoBodyState)
        {
            if (twoBodyState.Position is null || twoBodyState.Velocity is null || twoBodyState.GravitationalSourcePosition is null)
            {
                return BadRequest("Position, Velocity and Gravitational Source Position must be provided.");
            }
            string orbitName = !string.IsNullOrEmpty(twoBodyState.Name) ? twoBodyState.Name : new Guid().ToString();
            return Ok(await mediator.Send(new GetPositionedOrbitRequest(orbitName, twoBodyState.Position, twoBodyState.Velocity, twoBodyState.GravitationalSourcePosition, twoBodyState.GravitationalParameter)).ConfigureAwait(false));
        }
    }
}
