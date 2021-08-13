using diga.web.Models.Pagination;
using diga.web.Models.PlatformNotifications;
using diga.web.Models.Status;
using diga.web.Services.PlatformNotificationServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/notifications")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformNotificationController : BaseController
    {
        private readonly IPlatformNotificationService _platformNotificationService;

        public PlatformNotificationController(IPlatformNotificationService platformNotificationService)
            : base(null, null)
        {
            _platformNotificationService = platformNotificationService;
        }

        [HttpGet("{skip}/{take}")]
        public async Task<PlatformNotificationListDto> List(int skip, int take)
        {
            return await _platformNotificationService.List(UserId, skip, take);
        }

        [HttpPost("read/{notificationId}")]
        public async Task<ResponseData> SendMessage(int notificationId)
        {
            return await _platformNotificationService.Read(notificationId);
        }
    }
}
