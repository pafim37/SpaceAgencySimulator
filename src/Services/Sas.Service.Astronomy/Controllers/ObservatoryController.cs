using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sas.Service.Astronomy.DAL;
using Sas.Service.Astronomy.DTOs;
using Sas.Service.Astronomy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Service.Astronomy.Controllers
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
            Console.WriteLine("Hello World");
            var observatories = await _repository.GetAsync();
            var result = _mapper.Map<List<ObservatoryDTO>>(observatories);
            return Ok(result);
        }

        /// <summary>
        /// Get observatory by id or name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            if (Int32.TryParse(name, out int id))
            {
                var observatory = await _repository.GetAsync(id);
                var result = _mapper.Map<ObservatoryDTO>(observatory);
                return Ok(result);
            }
            else
            {
                var observatories = await _repository.GetAsync(name);
                var result = _mapper.Map<ObservatoryDTO>(observatories);
                return Ok(result);
            }
        }
    }
}
