using diga.web.Models.PlatformVerification;
using diga.web.Models.Status;
using diga.web.Services.PlatformVerificationServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/verifications")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformVerificationController : BaseController
    {
        private readonly IPlatformVerificationService _platformVerificationService;

        public PlatformVerificationController(IPlatformVerificationService platformVerificationService)
            : base(null, null)
        {
            _platformVerificationService = platformVerificationService;
        }

        [HttpPost("apply")]
        public async Task<ResponseData> Apply(PlatformVerificationApplyDto request)
        {
            return await _platformVerificationService.Apply(request, UserId);
        }
    }
}
