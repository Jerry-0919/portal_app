using diga.web.Models.PlatformContractPriorities;
using diga.web.Services.PlatformContractPriorityServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/contracts/priorities")]
    public class PlatformContractPriorityController : ControllerBase
    {
        private readonly IPlatformContractPriorityService _platformPriorityService;

        public PlatformContractPriorityController(IPlatformContractPriorityService platformPriorityService)
        {
            _platformPriorityService = platformPriorityService;
        }

        [HttpGet]
        public async Task<List<PlatformContractPriorityDto>> List()
        {
            return await _platformPriorityService.List();
        }
    }
}
