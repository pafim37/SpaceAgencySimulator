using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sas.Identity.Service.Data;
using Sas.Identity.Service.Entities;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

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

        [HttpPost("setcookie")]
        public async Task<ActionResult> SignInWithCookie()
        {
            HttpContext.Response.Cookies.Append("token","token1");
            return NoContent();
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Create(string username, string password)
        {
            var token = await GenerateToken(username);
            
            await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, token);
            return Ok(token);
        }

        private async Task<bool> IsValidUsernameAndPassword(string username, string password)
        {
            return await _context.Set<UserEntity>().Where(u => u.UserName.Equals(username)).AnyAsync();
        }

        public string GenerateToken(string userName)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("q#we1rty2u$io3plk4jhg5fds6a&z&x7cvb8n!m9@0");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("q#we1rty2u$io3plk4jhg5fds6a&z&x7cvb8n!m9@0");
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        //private async Task<dynamic> GenerateToken(string username)
        //{
        //    // var user = await _context.Set<UserEntity>().Where(u => u.UserName.Equals(username)).FirstAsync();
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, username),
        //    };

        //    var token = new JwtSecurityToken(
        //        new JwtHeader(
        //            new SigningCredentials(
        //                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("q#we1rty2u$io3plk4jhg5fds6a&z&x7cvb8n!m9@0")),
        //                SecurityAlgorithms.HmacSha256)),
        //        new JwtPayload(claims));

        //    var output = new
        //    {
        //        Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
        //        UserName = username,
        //        ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme))
        //    };

        //    return output;
        //}
    }
}
