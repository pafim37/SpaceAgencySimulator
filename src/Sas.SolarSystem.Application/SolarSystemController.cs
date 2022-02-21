using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sas.Domain.Bodies;
using Sas.SolarSystem.Application.Queries;

namespace Sas.SolarSystem.Application
{
    public class SolarSystemController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SolarSystemController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetSolarSystem()
        {
            var request = new GetAllBodiesQuery();
            var bodies = await _mediator.Send(request);

            var bodiesResult = _mapper.Map<IEnumerable<CelestialBody>>(bodies);

            Sas.Domain.SolarSystem solarSystem = new Sas.Domain.SolarSystem(bodiesResult.ToList());

            return Ok(solarSystem);
        }
    }
}