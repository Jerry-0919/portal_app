using diga.web.Models.PlatformLanguages;
using diga.web.Services.PlatformLanguageServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/languages")]
    public class PlatformLanguageController : ControllerBase
    {
        private readonly IPlatformLanguageService _platformLanguageService;

        public PlatformLanguageController(IPlatformLanguageService platformLanguageService)
        {
            _platformLanguageService = platformLanguageService;
        }

        [HttpGet]
        public async Task<List<PlatformLanguageDto>> List()
        {
            return await _platformLanguageService.List();
        }
    }
}
