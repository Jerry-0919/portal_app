using diga.web.Models.PlatformCountries;
using diga.web.Services.PlatformCountryServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/countries")]
    public class PlatformCountryController : ControllerBase
    {
        private readonly IPlatformCountryService _platformCountryService;

        public PlatformCountryController(IPlatformCountryService platformCountryService)
        {
            _platformCountryService = platformCountryService;
        }

        [HttpGet]
        public async Task<List<PlatformCountryDto>> List()
        {
            return await _platformCountryService.List();
        }
    }
}
