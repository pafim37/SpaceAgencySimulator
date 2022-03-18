using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sas.Identity.Service.Config;
using Sas.Identity.Service.Services;
using Sas.Identity.Service.Utils;

namespace Sas.Identity.Service.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next, IOptions<Settings> settings)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            context.Request.Cookies.TryGetValue("Authorization", out string token);
            var userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["UserEntity"] = userService.GetById(userId.Value);
            }

            await _next(context);
        }
    }
}
