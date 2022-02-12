using Microsoft.AspNetCore.Mvc;
using Sas.Service.Astronomy.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Service.Astronomy.Controllers
{
    [Route("observations")]
    [ApiController]
    public class ObservationController : ControllerBase
    {
        private readonly ObservationRepository _repository;

        public ObservationController(ObservationRepository repository)
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
