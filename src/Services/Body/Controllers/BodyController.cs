using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sas.Body.Service.DataTransferObject;
using Sas.Body.Service.Models.Entities;
using Sas.Body.Service.Repositories;

namespace Sas.Body.Service.Controllers
{
    [ApiController]
    [Route("body")]
    public class BodyController(IBodyRepository bodyRepository, IMapper mapper) : ControllerBase
    {
        private readonly CancellationTokenSource cancellationTokenSource = new();

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<BodyEntity> bodies = await bodyRepository.GetAllBodiesAsync(cancellationTokenSource.Token).ConfigureAwait(false);
            return Ok(mapper.Map<IEnumerable<BodyDto>>(bodies));
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            BodyEntity? bodyEntity = await bodyRepository.GetBodyByNameAsync(name, cancellationTokenSource.Token).ConfigureAwait(false);
            if (bodyEntity == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<BodyDto>(bodyEntity));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BodyDto body)
        {
            BodyEntity bodyDb = mapper.Map<BodyEntity>(body);
            await bodyRepository.CreateBodyAsync(bodyDb, cancellationTokenSource.Token).ConfigureAwait(false);
            return Created();
        }

    }
}
