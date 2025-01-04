using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sas.Body.Service.DataTransferObject;
using Sas.Body.Service.Exceptions;
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

        [HttpGet("/names")]
        public async Task<IActionResult> GetAllNames()
        {
            IEnumerable<string> names = await bodyRepository.GetAllBodiesNamesAsync(cancellationTokenSource.Token).ConfigureAwait(false);
            return Ok(names);
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
            ArgumentException.ThrowIfNullOrEmpty(body.Name);
            BodyEntity bodyDb = mapper.Map<BodyEntity>(body);
            try
            {
                await bodyRepository.CreateBodyAsync(bodyDb, cancellationTokenSource.Token).ConfigureAwait(false);
            }
            catch (BodyAlreadyExistsException e)
            {
                return StatusCode(409, new { message = e.Message });
            }
            return Created();
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] BodyDto body)
        {
            try
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(body.Name);
            }
            catch (Exception e)
            {
                return StatusCode(422, new { message = e.Message });
            }
            try
            {
                await bodyRepository.UpdateBodyAsync(body, cancellationTokenSource.Token).ConfigureAwait(false);
            }
            catch (NoBodyInDatabaseException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
            return Ok();
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            try
            {
                await bodyRepository.DeleteBodyAsync(name, cancellationTokenSource.Token).ConfigureAwait(false);
                return NoContent();
            }
            catch (NoBodyInDatabaseException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }
    }
}
