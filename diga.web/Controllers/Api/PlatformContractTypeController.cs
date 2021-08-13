using diga.web.Models.PlatformContractTypes;
using diga.web.Services.PlatformContractTypeServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/contracts/types")]
    public class PlatformContractTypeController : BaseController
    {
        private readonly IPlatformContractTypeService _platformContractTypeService;

        public PlatformContractTypeController(IPlatformContractTypeService platformContractTypeService)
            : base(null, null)
        {
            _platformContractTypeService = platformContractTypeService;
        }

        [HttpGet]
        public async Task<List<PlatformContractTypeDto>> List()
        {
            return await _platformContractTypeService.List();
        }
    }
}
