using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sas.Domain.Bodies;
using Sas.Identity.Service.Autorizations;
using Sas.SolarSystem.Service.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [AuthorizeAttribute]
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
