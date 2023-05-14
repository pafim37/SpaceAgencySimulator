using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sas.BodySystem.Service.DAL;
using Sas.BodySystem.Service.Documents;
using Sas.BodySystem.Service.DTOs;
using Sas.Domain.Models.Bodies;
using Sas.Mathematica.Service.Vectors;

namespace Sas.BodySystem.Service.Controllers
{
    [Route("body-system")]
    [ApiController]
    public class BodySystemController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IBodyRepository _repository;
        public BodySystemController(IBodyRepository repository, IMapper mapper, ILogger<BodySystemController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetBodySystem()
        {
            _logger.LogInformation("[GET] Body System Request");
            IEnumerable<BodyDocument> bodiesFromDb = await _repository.GetAllAsync().ConfigureAwait(false);
            IEnumerable<Body> bodies = _mapper.Map<IEnumerable<Body>>(bodiesFromDb);
            _logger.LogDebug("Successfully mapped bodies from database");
            Sas.Domain.Models.Bodies.BodySystem bodySystem = new(bodies);
            return Ok(bodySystem);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBodySystem([FromBody] IEnumerable<BodyDTO> bodyDtoList)
        {
            _logger.LogInformation("[POST] Body System Request");
            IEnumerable<BodyDocument> bodyDocumentList = _mapper.Map<IEnumerable<BodyDocument>>(bodyDtoList);
            await _repository.CreateOrUpdateAsync(bodyDocumentList).ConfigureAwait(false);
            List<Body> bodyList = CreateBodyList(bodyDtoList);
            Sas.Domain.Models.Bodies.BodySystem bodySystem = new(bodyList);
            _logger.LogInformation("Successfully handle request");
            return Ok(bodySystem);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFromBodySystem([FromBody] string bodyName)
        {
            _logger.LogInformation("[DELETE] Body System Request");
            await _repository.RemoveAsync(bodyName).ConfigureAwait(false);
            return NoContent();
        }

        private static List<Body> CreateBodyList(IEnumerable<BodyDTO>? bodyDtoList)
        {
            ArgumentNullException.ThrowIfNull(bodyDtoList, nameof(bodyDtoList));
            List<Body> bodyList = new();
            foreach (BodyDTO body in bodyDtoList)
            {
                Vector position = new Vector(body.Position.X, body.Position.Y, body.Position.Z);
                Vector velocity = new Vector(body.Velocity.X, body.Velocity.Y, body.Velocity.Z);
                bodyList.Add(new Body(body.Name, body.Mass, position, velocity, body.Radius));
            }
            return bodyList;
        }
    }
}
