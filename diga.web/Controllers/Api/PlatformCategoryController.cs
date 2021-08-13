using diga.web.Models.PlatformCategories;
using diga.web.Services.PlatformCategoryServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/categories")]
    public class PlatformCategoryController : ControllerBase
    {
        private readonly IPlatformCategoryService _platformCategoryService;

        public PlatformCategoryController(IPlatformCategoryService platformCategoryService)
        {
            _platformCategoryService = platformCategoryService;
        }

        [HttpGet]
        public async Task<List<PlatformCategoryDto>> List()
        {
            return await _platformCategoryService.List();
        }

        [HttpGet("published")]
        public async Task<List<PlatformCategoryDto>> ListPublished()
        {
            return await _platformCategoryService.ListPublished();
        }
    }
}
