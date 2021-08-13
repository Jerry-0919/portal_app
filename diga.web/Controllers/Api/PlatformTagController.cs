using diga.web.Models.PlatformTags;
using diga.web.Services.PlatformTagServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/tags")]
    public class PlatformTagController : ControllerBase
    {
        private readonly IPlatformTagService _platformTagService;

        public PlatformTagController(IPlatformTagService platformTagService)
        {
            _platformTagService = platformTagService;
        }

        [HttpGet]
        public async Task<List<PlatformTagDto>> List()
        {
            return await _platformTagService.List();
        }
    }
}
