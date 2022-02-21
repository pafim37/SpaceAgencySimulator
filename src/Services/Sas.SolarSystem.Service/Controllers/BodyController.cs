using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sas.SolarSystem.Service.DAL;
using Sas.SolarSystem.Service.Documents;

namespace Sas.SolarSystem.Service.Controllers
{
    [Route("bodies")]
    [ApiController]
    public class BodyController : ControllerBase
    {
        private readonly IBodyRepository _repository;

        public BodyController(IBodyRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var bodies = await _repository.GetAsync();
            if (bodies is null)
            {
                return NoContent();
            }
            return Ok(bodies);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            var body = await _repository.GetAsync(name);
            if (body is null)
            {
                return NoContent();
            }
            return Ok(body);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BodyDocument body)
        {
            if (body is null)
            {
                return NotFound();
            }

            await _repository.CreateAsync(body);
            return Created("", body);

        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            await _repository.RemoveAsync(name);
            return NoContent();
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> Update(string name, [FromBody] BodyDocument body)
        {
            await _repository.UpdateAsync(name, body);
            return NoContent();
        }
    }
}
