using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sas.Body.Service.Contexts;

namespace Sas.Body.Service.Controllers
{
    [ApiController]
    [Route("body")]
    public class BodyController(BodyContext context) : ControllerBase
    {
        private readonly BodyContext context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await context.Bodies
                .ToListAsync().ConfigureAwait(false));
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            return Ok(await context.Bodies.FirstOrDefaultAsync(body => body.Name == name).ConfigureAwait(false));
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            return Created();
        }

    }
}
