using diga.web.Models.Admin;
using diga.web.Models.Pagination;
using diga.web.Models.Status;
using diga.web.Services.AdminServices.AdminUserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminUserController : ControllerBase
    {
        private readonly IAdminUserService _userService;

        public AdminUserController(IAdminUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{skip}/{take}")]
        public async Task<PaginatedList<AdminUserDto>> ListUsers(int skip, int take)
        {
            return await _userService.GetPaginatedList(skip, take, new AdminPlatformUserFilterDto());
        }

        [HttpPost("{skip}/{take}")]
        public async Task<PaginatedList<AdminUserDto>> ListUsers(int skip, int take, AdminPlatformUserFilterDto filter)
        {
            return await _userService.GetPaginatedList(skip, take, filter);
        }

        [HttpGet("{id}")]
        public async Task<AdminUserFullDto> GetUser(int id)
        {
            return await _userService.Get(id);
        }

        [HttpPut("{id}")]
        public async Task<ResponseData> UpdateUser(int id, AdminUserEditDto request)
        {
            return await _userService.Update(id, request);
        }
    }
}
