using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sas.Identity.Service.Data;
using Sas.Identity.Service.Entities;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sas.Identity.Service.Controllers
{
    [Route("identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IdentityContext _context;

        public IdentityController(IdentityContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok("Dziala");
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Create(string username, string password)
        {
            return await GenerateToken(username);
        }

        private async Task<bool> IsValidUsernameAndPassword(string username, string password)
        {
            return await _context.Set<UserEntity>().Where(u => u.UserName.Equals(username)).AnyAsync();
        }

        private async Task<dynamic> GenerateToken(string username)
        {
            var user = await _context.Set<UserEntity>().Where(u => u.UserName.Equals(username)).FirstAsync();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysecret")),
                        SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            var output = new
            {
                Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserName = username
            };

            return output;
        }
    }
}
