using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sas.Identity.Service.Autorizations;
using Sas.Identity.Service.Config;
using Sas.Identity.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Identity.Service.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Settings _settings;

        public JwtMiddleware(RequestDelegate next, IOptions<Settings> settings)
        {
            _next = next;
            _settings = settings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            var nw = context.Request.Headers["Authorization"].Append();
            var token = nw.FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateJwtToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = userService.GetById(userId.Value);
            }

            await _next(context);
        }
    }
}
