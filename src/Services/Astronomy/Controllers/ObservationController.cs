using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sas.Astronomy.Service.DAL;
using Sas.Astronomy.Service.DTOs;
using Sas.Astronomy.Service.Models;
using Sas.Domain.Models.Observations;
using Sas.Domain.Models.Observatories;

namespace Sas.Astronomy.Service.Controllers
{
    [Route("observations")]
    [ApiController]
    public class ObservationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ObservationRepository _repository;

        public ObservationController(ObservationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all observations
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<ObservationEntity> observations = await _repository.GetAsync();
            List<ObservationDTO> result = _mapper.Map<List<ObservationDTO>>(observations);
            return result != null ? Ok(result) : NoContent();
        }

        /// <summary>
        /// Get all observations by object name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            IEnumerable<ObservationEntity> observations = await _repository.GetAsync(name);
            List<ObservationDTO> result = _mapper.Map<List<ObservationDTO>>(observations);
            return result != null ? Ok(result) : NoContent();
        }

        [HttpGet("extend/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ObservationEntity observation = await _repository.GetAsync(id);

            Observatory observatory = new Observatory(observation.Observatory.Name, observation.Observatory.LatitudeRad, observation.Observatory.LongitudeRad, observation.Observatory.Height);

            RadarObservation radarObservation = new RadarObservation(
                observatory,
                observation.ObjectName,
                observation.CreatedOn,
                observation.AzimuthRad,
                observation.AltitudeRad,
                observation.Distance
                );

            return Ok(radarObservation);
        }

        [HttpPost("create-observation")]
        public async Task<IActionResult> Create1([FromBody] ObservationDTO observationDto)
        {
            if (observationDto is not null)
            {
                ObservationEntity observation = _mapper.Map<ObservationEntity>(observationDto);
                await _repository.CreateAsync(observation);
                return Created("create-observation", observationDto);
            }
            else
                return NotFound();
        }

    }
}
