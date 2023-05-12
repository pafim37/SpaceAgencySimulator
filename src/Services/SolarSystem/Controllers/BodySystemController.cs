using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sas.BodySystem.Service.DAL;
using Sas.BodySystem.Service.Documents;
using Sas.BodySystem.Service.DTOs;
using Sas.Domain.Models.Bodies;
using Sas.Mathematica.Service.Vectors;
using System.Text.Json;

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
        public async Task<IActionResult> CreateBodySystem()
        {
            _logger.LogInformation("[POST] Body System Request");
            string requestBody = await GetRequestBodyFromStream().ConfigureAwait(false);
            if (requestBody.Equals(string.Empty, StringComparison.Ordinal))
            {
                _logger.LogDebug("No bodies were found in the request");
                return NoContent();
            }
            Sas.Domain.Models.Bodies.BodySystem bodySystem;
            List<Body> bodyList = GetBodyListFromRequestBody(requestBody);
            _logger.LogDebug("Successfully get bodies list from the request");
            if (bodyList.Any())
            {
                await _repository.CreateOrUpdateAsync(_mapper.Map<IEnumerable<BodyDocument>>(bodyList)).ConfigureAwait(false);
                bodySystem = new(bodyList);
                _logger.LogInformation("Successfully handle request");
                return Ok(bodySystem);
            }
            else
            {
                _logger.LogDebug("No bodies were found in the list");
                return Ok();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFromBodySystem([FromBody] string bodyName)
        {
            _logger.LogInformation("[DELETE] Body System Request");
            await _repository.RemoveAsync(bodyName).ConfigureAwait(false);
            return NoContent();
        }

        private async Task<string> GetRequestBodyFromStream()
        {
            string requestBody = string.Empty;
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                requestBody = await reader.ReadToEndAsync().ConfigureAwait(false);
            }
            return requestBody;
        }

        private List<Body> GetBodyListFromRequestBody(string requestBody)
        {
            JsonSerializerOptions option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            List<BodyDTO>? bodyDtoList = null;
            try
            {
                bodyDtoList = JsonSerializer.Deserialize<List<BodyDTO>>(requestBody, option);
            }
            catch (Exception ex) 
            { 
                try
                {
                    BodyDTO? body = JsonSerializer.Deserialize<BodyDTO>(requestBody, option);
                    ArgumentNullException.ThrowIfNull(body, nameof(body));
                    bodyDtoList = new() { body }; 
                }
                catch (Exception e)
                {
                    _logger.LogError($"An error occured when deserialized body request: {ex.Message}");
                    _logger.LogError($"Second attempt failed: {e.Message}");
                }
            }

            List<Body> bodyList = new();
            if (bodyDtoList is null)
            {
                return new List<Body>();
            }

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
