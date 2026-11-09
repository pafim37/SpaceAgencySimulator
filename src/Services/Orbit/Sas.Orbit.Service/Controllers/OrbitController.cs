using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sas.Orbit.Service.Exceptions;
using Sas.Orbit.Service.Models.DataTransferObject;
using Sas.Orbit.Service.Models.Requests;
using Sas.Orbit.Service.Orbits;
using Sas.Orbit.Service.Orbits.OrbitalElements;

namespace Sas.Orbit.Service.Controllers
{
    [ApiController]
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
            try
            {
                return Ok(await mediator.Send(new GetOrbitRequest(orbitalStateDto.Position, orbitalStateDto.Velocity, orbitalStateDto.GravitationalParameter)).ConfigureAwait(false));
            }
            catch (UnknownOrbitTypeException)
            {
                return BadRequest("Cannot predict orbit type. Unsupported value of eccentricity.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTwoBodyProblemOrbit([FromBody] TwoBodyState twoBodyState)
        {
            if (twoBodyState.Position is null || twoBodyState.Velocity is null || twoBodyState.GravitationalSourcePosition is null)
            {
                return BadRequest("Position, Velocity and Gravitational Source Position must be provided.");
            }
            string orbitName = !string.IsNullOrEmpty(twoBodyState.Name) ? twoBodyState.Name : new Guid().ToString();
            try
            {
                OrbitElements orbitElements = await mediator.Send(new GetOrbitRequest(twoBodyState.Position, twoBodyState.Velocity, twoBodyState.GravitationalParameter));
                return Ok(new PositionedOrbit(orbitName, twoBodyState.GravitationalSourcePosition, orbitElements));
            }
            catch (UnknownOrbitTypeException)
            {
                return BadRequest("Cannot predict orbit type. Unsupported value of eccentricity.");
            }
            catch (InvalidOperationException)
            {
                // TODO: Change this Exception to custom
                return BadRequest("Cannot predict center of the orbit");
            }
        }
    }
}
