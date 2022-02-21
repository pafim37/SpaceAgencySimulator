﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sas.Astronomy.Service.DAL;
using Sas.Astronomy.Service.DTOs;
using System.Text;

namespace Sas.Astronomy.Service.Controllers
{
    [Route("observatories")]
    [ApiController]
    public class ObservatoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ObservatoryRepository _repository;
        private readonly HttpClient _client;

        public ObservatoryController(ObservatoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _client = new HttpClient();
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
        [HttpGet("{IdOrName}")]
        public async Task<IActionResult> Get(string IdOrName)
        {
            ObservatoryDTO result;
            result = await GetObservatory(IdOrName);

            return result is not null ? Ok(result) : NoContent();
        }

        [HttpPost("{IdOrName}/create-instant-observation")]
        public async Task<IActionResult> CreateObservation(string IdOrName, [FromBody] ObservationCreateInstantDTO observationCreateInstantDto)
        {
            var observatory = await GetObservatory(IdOrName);
            var observationDto = _mapper.Map<ObservationDTO>(observationCreateInstantDto);
            observationDto.CreatedOn = DateTime.Now;
            observationDto.ObservatoryName = observatory.Name;

            var data = new StringContent(
                JsonConvert.SerializeObject(observationDto),
                Encoding.UTF8,
                "application/json"
                );

            string url = "https://localhost:5001/observation/create-observation";
            var response = await _client.PostAsync(url, data);

            return Ok(response.StatusCode);
        }
        
        private async Task<ObservatoryDTO> GetObservatory(string IdOrName)
        {
            ObservatoryDTO result;
            if (int.TryParse(IdOrName, out int id))
            {
                var observatory = await _repository.GetAsync(id);
                result = _mapper.Map<ObservatoryDTO>(observatory);
            }
            else
            {
                var observatory = await _repository.GetAsync(IdOrName);
                result = _mapper.Map<ObservatoryDTO>(observatory);
            }

            return result;
        }
    }
}
