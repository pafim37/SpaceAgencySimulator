﻿using AutoMapper;
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
        private readonly List<string> supportedBodyNames = new List<string>() { "Sun", "Earth", "Mars", "Moon", "Player" };

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<BodyEntity> bodies = await bodyRepository.GetAllBodiesAsync(cancellationTokenSource.Token).ConfigureAwait(false);
            return Ok(mapper.Map<IEnumerable<BodyDto>>(bodies));
        }

        [HttpGet("names")]
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

        [HttpPost("{name}")]
        public async Task<IActionResult> ChangeState(string name, [FromBody] bool newState)
        {
            try
            {
                await bodyRepository.ChangeBodyStateAsync(name, newState, cancellationTokenSource.Token);
            }
            catch (NoBodyInDatabaseException e)
            {
                return NotFound(e.Message);
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

        [HttpGet("supported-names")]
        public IActionResult GetSupportedNames()
        {
            return Ok(supportedBodyNames);
        }

        [HttpPost("defaults")]
        public async Task<IActionResult> Create([FromBody] List<string> bodynames)
        {
            IEnumerable<string> currentNames = await bodyRepository.GetAllBodiesNamesAsync(cancellationTokenSource.Token).ConfigureAwait(false);
            IEnumerable<string> newNames = bodynames.Except(currentNames);
            IEnumerable<string> commonNames = supportedBodyNames.Intersect(newNames);
            if (!commonNames.Any())
            {
                return StatusCode(409, new { message = "The request does not contain any supported body name or supported body already exists" });
            }
            IEnumerable<BodyEntity> bodies = CreateBodyEntitiesFromNames(bodynames);
            await bodyRepository.CreateRangeBodyAsync(bodies.ToList(), cancellationTokenSource.Token).ConfigureAwait(false);
            return Created("/", bodies);
        }

        private static IEnumerable<BodyEntity> CreateBodyEntitiesFromNames(List<string> names)
        {
            if (names.Contains("Sun"))
            {
                yield return new BodyEntity()
                {
                    Name = "Sun",
                    Enabled = true,
                    Mass = 100,
                    Radius = 5,
                    Position = new VectorEntity { X = 0, Y = 0, Z = 0 },
                    Velocity = new VectorEntity { X = 0, Y = 0, Z = 0 },
                };
            }
            if (names.Contains("Earth"))
            {
                yield return new BodyEntity()
                {
                    Name = "Earth",
                    Enabled = true,
                    Mass = 1,
                    Radius = 1,
                    Position = new VectorEntity { X = 0, Y = 0, Z = 0 },
                    Velocity = new VectorEntity { X = 0, Y = 0, Z = 0 },
                };
            }
            if (names.Contains("Moon"))
            {
                yield return new BodyEntity()
                {
                    Name = "Moon",
                    Enabled = true,
                    Mass = 0.01,
                    Radius = 0.5,
                    Position = new VectorEntity { X = 60, Y = 0, Z = 0 },
                    Velocity = new VectorEntity { X = 0, Y = 1.3, Z = 0 },
                };
            }
            if (names.Contains("Mars"))
            {
                yield return new BodyEntity()
                {
                    Name = "Mars",
                    Enabled = true,
                    Mass = 1,
                    Radius = 1,
                    Position = new VectorEntity { X = 0, Y = 50, Z = 0 },
                    Velocity = new VectorEntity { X = -1.4, Y = 0, Z = 0 },
                };
            }
            if (names.Contains("Player"))
            {
                yield return new BodyEntity()
                {
                    Name = "Player",
                    Enabled = true,
                    Mass = 0.01,
                    Radius = 0.001,
                    Position = new VectorEntity { X = 40, Y = 0, Z = 0 },
                    Velocity = new VectorEntity { X = 0, Y = 1.3, Z = 0 },
                };
            }
        }
    }
}
