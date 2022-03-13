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

        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymousAttribute]
        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery] AuthenticateRequest user)
        {
            var ipAddress = await GetIpAddress();
            var response = _userService.Authenticate(user, ipAddress);
            // await SetTokenCookie(response.RefreshToken);
            await SetTokenAuthorizationCookie(response.JwtToken);
            return Ok(response);
        }

        [AllowAnonymousAttribute]
        [HttpPost("login2")]
        public async Task<IActionResult> Login()
        {
            AuthenticateRequest user = new AuthenticateRequest() { Name = "pafim37", Password = "admin" };
            var ipAddress = await GetIpAddress();
            var response = _userService.Authenticate(user, ipAddress);
            await SetTokenCookie(response.RefreshToken);
            return Ok(response);
        }

        //[HttpGet("safe")]
        //public async Task<IActionResult> Login()
        //{
        //    AuthenticateRequest user = new AuthenticateRequest() { Name = "pafim37", Password = "admin"};
        //    var ipAddress = await GetIpAddress();
        //    var response = _userService.Authenticate(user, ipAddress);
        //    await SetTokenCookie(response.RefreshToken);
        //    return Ok(response);
        //}


        [HttpGet("public")]
        [AllowAnonymousAttribute]
        public async Task<IActionResult> ShowPublic()
        {
            return Ok("Public");
        }

        [HttpGet("secret")]
        [AuthorizeAttribute]
        public async Task<IActionResult> ShowSecret()
        {
            return Ok("Secret");
        }

        [HttpGet]
        public async Task<IActionResult> Free()
        {
            return Ok("Free");
        }

        private Task SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Expires = DateTime.UtcNow.AddDays(1),
            };
            Response.Cookies.Append("RefreshToken", token, cookieOptions);
            return Task.CompletedTask;
        }

        private Task SetTokenAuthorizationCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Expires = DateTime.UtcNow.AddDays(1),
            };
            Response.Cookies.Append("Authorization", token, cookieOptions);
            return Task.CompletedTask;
        }

        private async Task<string> GetIpAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
