using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
            var result = _mapper.Map<IEnumerable<BodyDTO>>(bodies);
            return Ok(result);
        }
    }
}
