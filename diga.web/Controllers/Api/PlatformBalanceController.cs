using diga.web.Models.PlatformBalances;
using diga.web.Services.PlatformBalanceServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/balances")]
    public class PlatformBalanceController : ControllerBase
    {
        private readonly IPlatformBalanceService _platformBalanceService;

        public PlatformBalanceController(IPlatformBalanceService platformBalanceService)
        {
            _platformBalanceService = platformBalanceService;
        }

        [HttpGet]
        public async Task<List<PlatformBalanceDto>> List()
        {
            return await _platformBalanceService.List();
        }
    }
}
