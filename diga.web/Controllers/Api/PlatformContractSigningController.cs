using diga.web.Models.PlatformContractSigning;
using diga.web.Models.Status;
using diga.web.Services.PlatformContractSigningServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/contracts/signing")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformContractSigningController : BaseController
    {
        private readonly IPlatformContractSigningService _platformContractSigningService;

        public PlatformContractSigningController(IPlatformContractSigningService platformContractSigningService)
            : base(null, null)
        {
            _platformContractSigningService = platformContractSigningService;
        }

        [HttpGet("{contractId}")]
        public async Task<PlatformContractSigningDto> Get(int contractId)
        {
            return await _platformContractSigningService.Get(contractId);
        }

        [HttpPatch("edit/{contractId}")]
        public async Task<ResponseData> Edit(int contractId, PlatformContractSigningDto request)
        {
            return await _platformContractSigningService.Edit(UserId, contractId, request);
        }

        [HttpPatch("approve/{contractId}")]
        public async Task<ResponseData> Approve(int contractId)
        {
            return await _platformContractSigningService.Approve(UserId, contractId);
        }
    }
}
