using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sas.Mathematica;
using Sas.SolarSystem.Service.DAL;
using Sas.SolarSystem.Service.DTOs;

namespace Sas.SolarSystem.Service.Controllers
{
    [Route("bodies")]
    [ApiController]
    public class BodyController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBodyRepository _repository;

        public BodyController(IBodyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var bodies = await _repository.GetAsync();
            if (bodies == null)
            {
                return NoContent();
            }
            var result = _mapper.Map<IEnumerable<BodyDTO>>(bodies);
            return Ok(result);
        }

        [HttpGet("barycentrum")]
        public async Task<IActionResult> GetBarycentrum()
        {
            var bodies = await _repository.GetAsync();
            if (bodies == null)
            {
                return NoContent();
            }

            double sumMass = 0;
            double x = 0;
            double y = 0;
            double z = 0;

            foreach (var body in bodies)
            {
                x += body.Mass * body.AbsolutePosition.X;
                y += body.Mass * body.AbsolutePosition.Y;
                z += body.Mass * body.AbsolutePosition.Z;
                sumMass += body.Mass;
            }

            var result = 1.0 / sumMass * new Vector(x, y, z);
            return Ok(result);
        }
    }
}
