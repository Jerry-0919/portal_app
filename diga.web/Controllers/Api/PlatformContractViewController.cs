using diga.web.Models.Status;
using diga.web.Services.PlatformContractViewServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/contracts/views")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformContractViewController : BaseController
    {
        private readonly IPlatformContractViewService _platformContractViewService;

        public PlatformContractViewController(IPlatformContractViewService platformContractViewService)
            : base(null, null)
        {
            _platformContractViewService = platformContractViewService;
        }

        [HttpPost("{contractId}")]
        public async Task<ResponseData> Add(int contractId)
        {
            return await _platformContractViewService.Add(UserId, contractId);
        }
    }
}
