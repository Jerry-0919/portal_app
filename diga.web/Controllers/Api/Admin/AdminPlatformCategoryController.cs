using diga.web.Models.Admin;
using diga.web.Models.Pagination;
using diga.web.Models.Status;
using diga.web.Services.AdminServices.AdminPlatformCategoryServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminPlatformCategoryController : ControllerBase
    {
        private readonly IAdminPlatformCategoryService _platformCategoryService;

        public AdminPlatformCategoryController(
            IAdminPlatformCategoryService platformCategoryService)
        {
            _platformCategoryService = platformCategoryService;
        }

        [HttpGet("platform/categories/{skip}/{take}")]
        public async Task<PaginatedList<AdminPlatformCategoryDto>> ListPlatoformCategories(int skip, int take)
        {
            return await _platformCategoryService.GetPaginatedList(skip, take);
        }

        [HttpGet("platform/categories/{id}")]
        public async Task<AdminPlatformCategoryDto> GetPlatoformCategory(int id)
        {
            return await _platformCategoryService.Get(id);
        }

        [HttpPost("platform/categories")]
        public async Task<ResponseData> AddPlatformCategory(AdminPlatformCategoryAddDto request)
        {
            return await _platformCategoryService.Add(request);
        }

        [HttpPut("platform/categories/{id}")]
        public async Task<ResponseData> UpdatePlatoformCategory(int id, AdminPlatformCategoryEditDto request)
        {
            return await _platformCategoryService.Update(id, request);
        }

        [HttpDelete("platform/categories/{id}")]
        public async Task<ResponseData> RemovePlatoformCategory(int id)
        {
            return await _platformCategoryService.Remove(id);
        }
    }
}
