using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sas.Domain.Bodies;
using Sas.SolarSystem.Service.DAL;

namespace Sas.SolarSystem.Service.Controllers
{
    [Route("solar-system")]
    [Authorize]
    [ApiController]
    public class SolarSystemController : ControllerBase
    {
        private readonly ICelestialBodyRepository _repository;
        private readonly IMapper _mapper;

        public SolarSystemController(ICelestialBodyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> LoadSolarSystem()
        {
            var bodiesFromDatabase = await _repository.GetAsync();
            var bodies = _mapper.Map<IEnumerable<CelestialBody>>(bodiesFromDatabase);
            Sas.Domain.SolarSystem solarSystem = new Sas.Domain.SolarSystem(bodies.ToList());
            return Ok(bodies);
        }
    }
}
