using Microsoft.AspNetCore.Mvc;
using Sas.Domain.Users;
using Sas.Identity.Service.Attributes;
using Sas.Identity.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sas.Identity.Service.Controllers
{
    [Route(route)]
    [ApiController]
    [AuthorizeAttribute]
    public class UserController : ControllerBase
    {
        private const string route = "users";
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("all")]
        [AuthorizeRoleBasedAttribute(Role.Admin)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [AuthorizeRoleBasedAttribute(Role.Admin)]
        public async Task<IActionResult> Get(int id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }


    }
}
