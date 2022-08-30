using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sas.Identity.Service.Autorizations;
using Sas.SolarSystem.Service.DAL;
using Sas.SolarSystem.Service.Models;

namespace Sas.SolarSystem.Service.Controllers
{
    [Route("solar-system")]
    [ApiController]
    public class SolarSystemController : ControllerBase
    {
        private readonly IBodyRepository _repository;
        private readonly IMapper _mapper;

        public SolarSystemController(IBodyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //[AuthorizeAttribute]
        //[HttpGet]
        //public async Task<IActionResult> LoadSolarSystem()
        //{
        //    var bodiesFromDatabase = await _repository.GetAsync();
        //    var bodies = _mapper.Map<IEnumerable<Body>>(bodiesFromDatabase);
        //    Sas.Domain.SolarSystem solarSystem = new Sas.Domain.SolarSystem(bodies.ToList());
        //    return Ok(bodies);
        //}
    }
}
