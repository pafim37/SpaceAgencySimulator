using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sas.Domain.Models.Bodies;
using Sas.SolarSystem.Service.DAL;

namespace Sas.SolarSystem.Service.Controllers
{
    [Route("solar-system")]
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
            IEnumerable<Documents.CelestialBodyDocument> bodiesFromDatabase = await _repository.GetAsync();
            IEnumerable<Body> bodies = _mapper.Map<IEnumerable<Body>>(bodiesFromDatabase);
            Sas.Domain.SolarSystem solarSystem = new Sas.Domain.SolarSystem(bodies.ToList());
            return Ok(bodies);
        }
    }
}
