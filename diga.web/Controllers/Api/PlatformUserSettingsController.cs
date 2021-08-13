using diga.web.Models.PlatformUserSettings;
using diga.web.Models.Status;
using diga.web.Services.PlatformUserSettingsServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/users/settings")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformUserSettingsController : BaseController
    {
        private readonly IPlatformUserSettingsService _platformUserSettingsService;

        public PlatformUserSettingsController(IPlatformUserSettingsService platformUserSettingsService)
            : base(null, null)
        {
            _platformUserSettingsService = platformUserSettingsService;
        }

        [HttpGet("filter")]
        public async Task<PlatformUserFilterSettingsDto> GetFilter()
        {
            return await _platformUserSettingsService.GetFilter(UserId);
        }

        [HttpPut("filter")]
        public async Task<ResponseData> UpdateFilter(PlatformUserFilterSettingsDto request)
        {
            return await _platformUserSettingsService.UpdateFilter(UserId, request);
        }
    }
}
