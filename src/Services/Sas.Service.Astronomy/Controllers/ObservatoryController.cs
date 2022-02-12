using Microsoft.AspNetCore.Mvc;
using Sas.Service.Astronomy.DAL;
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
        private readonly ObservatoryRepository _repository;

        public ObservatoryController(ObservatoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _repository.GetAsync());
        } 
    }
}
