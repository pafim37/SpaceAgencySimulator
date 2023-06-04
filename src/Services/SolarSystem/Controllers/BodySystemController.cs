using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sas.BodySystem.Service.DAL;
using Sas.BodySystem.Service.Documents;
using Sas.BodySystem.Service.DTOs;
using Sas.Domain.Models.Bodies;

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
        public async Task<IActionResult> GetBodySystem([FromQuery] double gravitationalConstant)
        {
            _logger.LogInformation("[GET] Body System Request");
            IEnumerable<BodyDocument> bodiesFromDb = await _repository.GetAllAsync().ConfigureAwait(false);
            IEnumerable<Body> bodies = _mapper.Map<IEnumerable<Body>>(bodiesFromDb);
            _logger.LogDebug("Successfully mapped bodies from database");
            Sas.Domain.Models.Bodies.BodySystem bodySystem = new(bodies, gravitationalConstant);
            BodySystemOutputData bodySystemDto = _mapper.Map<BodySystemOutputData>(bodySystem);
            _logger.LogInformation("Successfully handle request");
            return Ok(bodySystemDto);
        }

        [HttpGet("bodies")]
        public async Task<IActionResult> GetBodies()
        {
            _logger.LogInformation("[GET] Body Request");
            IEnumerable<BodyDocument> bodiesFromDb = await _repository.GetAllAsync().ConfigureAwait(false);
            IEnumerable<BodyDTO> bodies = _mapper.Map<IEnumerable<BodyDTO>>(bodiesFromDb);
            return Ok(bodies);
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] IEnumerable<BodyDTO> bodies) // TODO: no longer need gravitational const here
        {
            _logger.LogInformation("[POST] Save Bodies Request");
            await SaveBodies(bodies).ConfigureAwait(false);
            _logger.LogInformation("Successfully Save Data");
            return Ok();
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] BodySystemInputData inputData)
        {
            return CreateBodySystem(inputData.Bodies, inputData.GravitationalConstant);
        }

        [HttpPost("synchronize")]
        public async Task<IActionResult> Synchronize([FromBody] BodySystemInputData inputData) // TODO: no longer need gravitational const here
        {
            _logger.LogInformation("[POST] Synchronize Request");
            IEnumerable<BodyDocument> bodiesFromDb = await _repository.GetAllAsync().ConfigureAwait(false);
            IEnumerable<BodyDTO> commonBodies = inputData.Bodies.Join(bodiesFromDb, body1 => body1.Name, body2 => body2.Name, (body1, body2) => body1);
            IEnumerable<BodyDocument> bodiesToRemove = bodiesFromDb.Where(body1 => !inputData.Bodies.Any(body2 => body2.Name == body1.Name));
            await _repository.RemoveManyAsync(bodiesToRemove.Select(x => x.Name)).ConfigureAwait(false);
            await SaveBodies(commonBodies).ConfigureAwait(false);
            return CreateBodySystem(commonBodies, inputData.GravitationalConstant);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFromBodySystem([FromBody] string bodyName)
        {
            _logger.LogInformation("[DELETE] Body System Request");
            await _repository.RemoveAsync(bodyName).ConfigureAwait(false);
            return NoContent();
        }

        private IActionResult CreateBodySystem(IEnumerable<BodyDTO> bodies, double gravitationalConst)
        {
            _logger.LogInformation("[POST] Body System Request");
            IEnumerable<Body> bodyList = _mapper.Map<IEnumerable<Body>>(bodies);
            Sas.Domain.Models.Bodies.BodySystem bodySystem = new(bodyList, gravitationalConst);
            BodySystemOutputData bodySystemDto = _mapper.Map<BodySystemOutputData>(bodySystem);
            _logger.LogInformation("Successfully create body system");
            return Ok(bodySystemDto);
        }

        private async Task SaveBodies(IEnumerable<BodyDTO> bodies)
        {
            IEnumerable<BodyDocument> bodyDocumentList = _mapper.Map<IEnumerable<BodyDocument>>(bodies);
            await _repository.CreateOrReplaceAsync(bodyDocumentList).ConfigureAwait(false);
        }
    }
}
