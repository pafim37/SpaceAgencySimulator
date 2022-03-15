using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sas.Identity.Service.Models;

namespace Sas.Identity.Service.Autorizations
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeRoleBasedAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly Role _role;
        public AuthorizeRoleBasedAttribute(Role role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            var user = (UserEntity)context.HttpContext.Items["UserEntity"];
            if (user == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            // allow administrator
            else if (user.Roles.Where(u => u.Role == Role.Admin).Any())
            {
                return;
            }
            else if (user.Roles.Where(u => u.Role == _role).Any())
            {
                return;
            }
            else
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
