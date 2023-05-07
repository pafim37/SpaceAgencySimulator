using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sas.Domain.Models.Bodies;
using Sas.BodySystem.Service.DAL;
using Sas.Mathematica.Service.Vectors;
using Sas.Mathematica.Service;

namespace Sas.BodySystem.Service.Controllers
{
    [Route("body-system")]
    [ApiController]
    public class BodySystemController : ControllerBase
    {
        private readonly IBodyRepository _repository;
        private readonly IMapper _mapper;

        public BodySystemController(IBodyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetSolarSystem()
        {
            IEnumerable<Documents.BodyDocument> bodiesFromDatabase = await _repository.GetAsync();
            IEnumerable<Body> bodies = _mapper.Map<IEnumerable<Body>>(bodiesFromDatabase);
            Sas.Domain.Models.Bodies.BodySystem solarSystem = new Sas.Domain.Models.Bodies.BodySystem(bodies.ToList());
            return Ok(solarSystem);
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            double M = 10000000000;
            double rx = 100;
            Vector smallVelocity = new Vector(0, Math.Sqrt(Constants.G * M / (rx)), 0);
            Body smallBody = new Body("Small Body", 10, new Vector(rx+200, 320, 0), smallVelocity, 7);
            Body bigBody = new Body("Big body", M, new Vector(200, 320, 0), Vector.Zero, 13);
            List<Body> bodies = new List<Body>() { bigBody, smallBody };
            Sas.Domain.Models.Bodies.BodySystem bodySystem = new Sas.Domain.Models.Bodies.BodySystem(bodies);
            return Ok(bodySystem);
        }

        [HttpGet("test2")]
        public async Task<IActionResult> Test2()
        {
            var obj = new
            {
                Name = "One"
            };
            return Ok(obj);
        }
    }
}
