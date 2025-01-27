using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sas.Body.Service.DataTransferObject;
using Sas.Body.Service.Exceptions;
using Sas.Body.Service.Models.Entities;
using Sas.Body.Service.Notifications;
using Sas.Body.Service.Repositories;

namespace Sas.Body.Service.Controllers
{
    [ApiController]
    [Route("body")]
    public class BodyController(IBodyRepository bodyRepository, NotificationClient notificationService, IMapper mapper) : ControllerBase
    {
        private const string SignalRClientIdHeaderName = "X-SAS-SignalRClientId";
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly List<string> supportedBodyNames = new List<string>() { "Sun", "Mercury", "Wenus", "Earth", "Mars", "Moon", "Player" };

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
        public async Task<IActionResult> Create([FromBody] NewBodyDto body, [FromHeader(Name = SignalRClientIdHeaderName)] string? signalRConnectionId)
        {
            ArgumentException.ThrowIfNullOrEmpty(body.Name);
            BodyEntity bodyDb = mapper.Map<BodyEntity>(body);
            try
            {
                await bodyRepository.CreateBodyAsync(bodyDb, cancellationTokenSource.Token).ConfigureAwait(false);
                await notificationService.SendBodyDatabaseChangedNotification(signalRConnectionId, cancellationTokenSource.Token);
                return Created("/", body);
            }
            catch (BodyAlreadyExistsException e)
            {
                return StatusCode(409, new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { message = e.Message });
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] BodyDto body, [FromHeader(Name = SignalRClientIdHeaderName)] string? signalRConnectionId)
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
                BodyEntity updatedBody = await bodyRepository.UpdateBodyAsync(body, cancellationTokenSource.Token).ConfigureAwait(false);
                await notificationService.SendBodyDatabaseChangedNotification(signalRConnectionId, cancellationTokenSource.Token);
                return Ok(updatedBody);
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

        [HttpPost("{name}")]
        public async Task<IActionResult> ChangeState(string name, [FromBody] bool newState, [FromHeader(Name = SignalRClientIdHeaderName)] string? signalRConnectionId)
        {
            try
            {
                BodyEntity body = await bodyRepository.ChangeBodyStateAsync(name, newState, cancellationTokenSource.Token);
                await notificationService.SendBodyDatabaseChangedNotification(signalRConnectionId, cancellationTokenSource.Token);
                return Ok(body);
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

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name, [FromHeader(Name = SignalRClientIdHeaderName)] string? signalRConnectionId)
        {
            try
            {
                BodyEntity body = await bodyRepository.DeleteBodyAsync(name, cancellationTokenSource.Token).ConfigureAwait(false);
                await notificationService.SendBodyDatabaseChangedNotification(signalRConnectionId, cancellationTokenSource.Token);
                return Ok(body);
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
        public async Task<IActionResult> Create([FromBody] List<string> bodynames, [FromHeader(Name = SignalRClientIdHeaderName)] string? signalRConnectionId)
        {
            IEnumerable<string> currentNames = await bodyRepository.GetAllBodiesNamesAsync(cancellationTokenSource.Token).ConfigureAwait(false);
            IEnumerable<string> newNames = bodynames.Except(currentNames);
            IEnumerable<string> commonNames = supportedBodyNames.Intersect(newNames);
            if (!commonNames.Any())
            {
                return StatusCode(409, new { message = "The request does not contain any supported body name or supported body already exists" });
            }
            IEnumerable<NewBodyDto> newBodies = CreateBodyEntitiesFromNames(bodynames);
            List<BodyEntity> bodiesDto = mapper.Map<List<BodyEntity>>(newBodies);
            await bodyRepository.CreateRangeBodyAsync(bodiesDto, cancellationTokenSource.Token).ConfigureAwait(false);
            await notificationService.SendBodyDatabaseChangedNotification(signalRConnectionId, cancellationTokenSource.Token);
            return Created("/", bodiesDto);
        }

        private static IEnumerable<NewBodyDto> CreateBodyEntitiesFromNames(List<string> names)
        {
            float sunMass = 332950;
            if (names.Contains("Sun"))
            {
                yield return new NewBodyDto()
                {
                    Name = "Sun",
                    Enabled = true,
                    Mass = sunMass,
                    Radius = 5,
                    Position = new VectorDto { X = 0, Y = 0, Z = 0 },
                    Velocity = new VectorDto { X = 0, Y = 0, Z = 0 },
                };
            }
            if (names.Contains("Mercury"))
            {
                double distance = 38;
                double velocity = Math.Round(Math.Sqrt(sunMass / distance), 2);
                yield return new NewBodyDto()
                {
                    Name = "Mercury",
                    Enabled = true,
                    Mass = 0.0552,
                    Radius = 1,
                    Position = new VectorDto { X = distance, Y = 0, Z = 0 },
                    Velocity = new VectorDto { X = 0, Y = velocity, Z = 0 },
                };
            }
            if (names.Contains("Wenus"))
            {
                double distance = 72;
                double velocity = Math.Round(Math.Sqrt(sunMass / distance), 2);
                yield return new NewBodyDto()
                {
                    Name = "Wenus",
                    Enabled = true,
                    Mass = 0.8149,
                    Radius = 1,
                    Position = new VectorDto { X = distance, Y = 0, Z = 0 },
                    Velocity = new VectorDto { X = 0, Y = velocity, Z = 0 },
                };
            }
            if (names.Contains("Earth"))
            {
                double distance = 100;
                double velocity = Math.Round(Math.Sqrt(sunMass / distance), 2);
                yield return new NewBodyDto()
                {
                    Name = "Earth",
                    Enabled = true,
                    Mass = 1,
                    Radius = 1,
                    Position = new VectorDto { X = distance, Y = 0, Z = 0 },
                    Velocity = new VectorDto { X = 0, Y = velocity, Z = 0 },
                };
            }
            if (names.Contains("Moon"))
            {
                double diff = 8;
                double distance = diff;
                double velocity = Math.Round(Math.Sqrt(1 / distance), 2); // relative to Earth
                yield return new NewBodyDto()
                {
                    Name = "Moon",
                    Enabled = true,
                    Mass = 0.01,
                    Radius = 0.5,
                    Position = new VectorDto { X = 100, Y = diff, Z = 0 }, // relative to Earth
                    Velocity = new VectorDto { X = -velocity, Y = -Math.Round(Math.Sqrt(sunMass / 100), 2), Z = 0 },
                };
            }
            if (names.Contains("Mars"))
            {
                double distance = 152;
                double velocity = Math.Round(Math.Sqrt(sunMass / distance), 2);
                yield return new NewBodyDto()
                {
                    Name = "Mars",
                    Enabled = true,
                    Mass = 1,
                    Radius = 1,
                    Position = new VectorDto { X = distance, Y = 0, Z = 0 },
                    Velocity = new VectorDto { X = 0, Y = velocity, Z = 0 },
                };
            }
            if (names.Contains("Player"))
            {
                double diff = 4;
                double distance = 100 + diff;
                double velocity = Math.Round(Math.Sqrt(sunMass / distance), 2);
                yield return new NewBodyDto()
                {
                    Name = "Player",
                    Enabled = true,
                    Mass = 0.01,
                    Radius = 0.001,
                    Position = new VectorDto { X = distance - diff, Y = diff, Z = 0 },
                    Velocity = new VectorDto { X = 0, Y = velocity, Z = 0 },
                };
            }
        }
    }
}
