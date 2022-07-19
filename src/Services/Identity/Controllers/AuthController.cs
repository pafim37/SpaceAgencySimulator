using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sas.Identity.Service.Autorizations;
using Sas.Identity.Service.Generators;
using Sas.Identity.Service.Models;
using Sas.Identity.Service.Services;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Sas.Identity.Service.Controllers
{
    [Route("identity")]
    [ApiController]
    [AuthorizeAttribute]
    public class AuthController : ControllerBase
    {
        private const string AuthorizationCookieName = "Authorization";
        private IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("login")]
        [AllowAnonymousAttribute]
        public async Task<IActionResult> Login([FromQuery] AuthenticateRequest user)
        {
            var response = await _userService.AuthenticateAsync(user);
            SetTokenCookie(response.JwtToken);
            return Ok($"Hello {user.Name}");
        }

        [HttpGet("logout")]
        [AuthorizeAttribute]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(AuthorizationCookieName);
            return NoContent();
        }

        [HttpGet("register")]
        [AllowAnonymousAttribute]
        public async Task<IActionResult> Register([FromQuery] AuthenticateRequest user)
        {
            // check of user existance in db
            var userFromDb = await _userService.GetByNameAsync(user.Name);

            if (userFromDb != null)
            {
                return NoContent();
            }
            else
            {
                var salt = StringGenerator.Generate(6);
                var role = new RoleEntity() { Role = Role.NoPrivilege};
                UserEntity userEntity = new UserEntity()
                {
                    Name = user.Name,
                    PasswordHash = BCryptNet.HashPassword(user.Password + salt),
                    Salt = salt,
                    Roles = new() { role }
                };
                await _userService.CreateAsync(userEntity);
                return await Login(user);
            }
        }
    
        [HttpGet("test")]
        [AuthorizeAttribute]
        public IActionResult Test()
        {
            return Ok("You are authorized :)");
        }

        [HttpGet("all")]
        [AuthorizeRoleBasedAttribute(Role.Admin)]
        public async Task<IActionResult> ShowAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(1)
            };
            Response.Cookies.Append(AuthorizationCookieName, token, cookieOptions);
        }
    }
}
