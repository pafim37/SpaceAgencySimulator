using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sas.Identity.Service.Autorizations;
using Sas.Identity.Service.Models;
using Sas.Identity.Service.Services;

namespace Sas.Identity.Service.Controllers
{
    [Route("identity")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private const string AuthorizationCookieName = "Authorization";
        private IUserService _userService;

        public UserController(IUserService userService)
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

        [HttpGet("test")]
        [AuthorizeAttribute]
        public IActionResult Test()
        {
            return Ok("You are authorized :)");
        }

        [HttpGet("all")]
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
