using diga.web.Models.Roles;
using diga.web.Models.Status;
using diga.web.Services.RolesServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/roles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService) : base(null, null)
        {
            _roleService = roleService;
        }

        [HttpGet("users")]
        public async Task<List<UserRoleDto>> List()
        {
            return await _roleService.List(UserId);
        }

        [HttpPost("users/{role}")]
        public async Task<ResponseData> AddUserRole(string role)
        {
            return await _roleService.AddUserRole(UserId, role);
        }

        [HttpDelete("users/{role}")]
        public async Task<ResponseData> RemoveUserRole(string role)
        {
            return await _roleService.RemoveUserRole(UserId, role);
        }
    }
}
