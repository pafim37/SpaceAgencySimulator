using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sas.Astronomy.Service.DAL;
using Sas.Astronomy.Service.DTOs;

namespace Sas.Astronomy.Service.Controllers
{
    [Route("observatories")]
    [ApiController]
    public class ObservatoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ObservatoryRepository _repository;

        public ObservatoryController(ObservatoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all observatories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var observatories = await _repository.GetAsync();
            var result = _mapper.Map<List<ObservatoryDTO>>(observatories);
            return result != null ? Ok(result) : NoContent();
        }

        /// <summary>
        /// Get observatory by id or name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            ObservatoryDTO result;

            if (int.TryParse(name, out int id))
            {
                var observatory = await _repository.GetAsync(id);
                result = _mapper.Map<ObservatoryDTO>(observatory);
            }
            else
            {
                var observatory = await _repository.GetAsync(name);
                result = _mapper.Map<ObservatoryDTO>(observatory);
            }

            return result != null ? Ok(result) : NoContent();
        }
    }
}
