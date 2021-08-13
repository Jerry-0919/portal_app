using diga.web.Models.PlatformCities;
using diga.web.Services.PlatformCityServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/cities")]
    public class PlatformCityController : ControllerBase
    {
        private readonly IPlatformCityService _platformCityService;

        public PlatformCityController(IPlatformCityService platformCityService)
        {
            _platformCityService = platformCityService;
        }

        [HttpGet]
        public async Task<List<PlatformCityDto>> List()
        {
            return await _platformCityService.List();
        }
    }
}
