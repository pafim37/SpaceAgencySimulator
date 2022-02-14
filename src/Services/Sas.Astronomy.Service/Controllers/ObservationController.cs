using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sas.Astronomy.Service.DAL;
using Sas.Astronomy.Service.DTOs;
using Sas.Astronomy.Service.Models;
using Sas.Domain;
using Sas.Domain.Observations;

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
            var observations = await _repository.GetAsync();

            return Ok(observations);
        }

        /// <summary>
        /// Get all observations by object name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var observations = await _repository.GetAsync(name);
            List<ObservationDTO> result = new List<ObservationDTO>();

            HttpClient client = new HttpClient();
            foreach (var observation in observations)
            {
                var observatoryId = observation.Id;
                var endpoint = $"https://localhost:5001/observatories/{observatoryId}";
                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                ObservatoryEntity observatory = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservatoryEntity>(responseBody);
                observation.Observatory = observatory;

                var observationDto = _mapper.Map<ObservationDTO>(observation);
                result.Add(observationDto);

            }
            return Ok(result);
        }

        [HttpGet("extend/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var observationEntity = await _repository.GetAsync(id);

            HttpClient client = new HttpClient();
            var observatoryId = observationEntity.ObservatoryId;
            var endpoint = $"https://localhost:5001/observatories/{observatoryId}";
            var response = await client.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            ObservatoryEntity observatoryEntity = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservatoryEntity>(responseBody);

            observationEntity.Observatory = observatoryEntity;

            Observatory observatory = new Observatory(observatoryEntity.Name, observatoryEntity.LatitudeRad, observatoryEntity.LongitudeRad, observatoryEntity.Height);

            RadarObservation radarObservation = new RadarObservation(
                observatory, observationEntity.ObjectName,
                observationEntity.CreatedOn,
                observationEntity.AzimuthRad,
                observationEntity.AltitudeRad,
                observationEntity.Distance
                );

            return Ok(radarObservation);
        }
    }
}
