using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sas.Body.Service.Contexts;
using Sas.Body.Service.DataTransferObject;
using Sas.Mathematica.Service.Vectors;

namespace Sas.Body.Service.Controllers
{
    [ApiController]
    [Route("body")]
    public class BodyController(BodyContext context, IMapper mapper) : ControllerBase
    {
        private readonly BodyContext context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bodies = await context.Bodies.ToListAsync().ConfigureAwait(false);
            return Ok(mapper.Map<IEnumerable<BodyDto>>(bodies));
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var body = await context.Bodies.FirstOrDefaultAsync(body => body.Name == name).ConfigureAwait(false);
            return Ok(mapper.Map<Vector>(body.Position));
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            return Created();
        }

    }
}
