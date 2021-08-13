using diga.web.Models.Admin;
using diga.web.Models.Status;
using diga.web.Services.AdminServices.AdminPlatformVerificationServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api.Admin
{
    [ApiController]
    [Route("api/admin/platform/verifications")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminPlatformVerificationController : BaseController
    {
        private readonly IAdminPlatformVerificationService _platformVerificationService;

        public AdminPlatformVerificationController(IAdminPlatformVerificationService platformVerificationService)
            : base(null, null)
        {
            _platformVerificationService = platformVerificationService;
        }

        [HttpPost("check")]
        public async Task<ResponseData> Check(AdminUserVerificationCheckDto request)
        {
            return await _platformVerificationService.Check(request);
        }
    }
}
