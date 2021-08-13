using diga.web.Models.PlatformVATs;
using diga.web.Services.PlatformVATServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/vats")]
    public class PlatformVATController : ControllerBase
    {
        private readonly IPlatformVATService _platformVATService;

        public PlatformVATController(IPlatformVATService platformVATService)
        {
            _platformVATService = platformVATService;
        }

        [HttpGet]
        public async Task<List<PlatformVATDto>> List()
        {
            return await _platformVATService.List();
        }
    }
}
