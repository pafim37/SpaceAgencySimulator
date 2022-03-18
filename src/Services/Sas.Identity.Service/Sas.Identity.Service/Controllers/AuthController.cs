using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sas.Domain.Users;
using Sas.Identity.Service.Attributes;
using Sas.Identity.Service.Generators;
using Sas.Identity.Service.Models;
using Sas.Identity.Service.Models.Entities;
using Sas.Identity.Service.Services;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Sas.Identity.Service.Controllers
{
    [Route(route)]
    [ApiController]
    [AuthorizeAttribute]
    public class AuthController : ControllerBase
    {
        private const string route = "identity";
        private const string CookieName = "Authorization";
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
            Response.Cookies.Delete(CookieName);
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
                var result = _userService.CreateAsync(userEntity);
                if (result.IsCompletedSuccessfully)
                {
                    return await Login(user);
                }
                else
                {
                    throw new Exception("User cannot be created");
                }
            }
        }
    
        [HttpGet("test")]
        [AuthorizeAttribute]
        public IActionResult Test()
        {
            return Ok("You are authorized :)");
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(1),
                Secure = true
            };
            Response.Cookies.Append(CookieName, token, cookieOptions);
        }
    }
}
