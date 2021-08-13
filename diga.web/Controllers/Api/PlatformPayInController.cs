using diga.web.Models.PlatformPayIns;
using diga.web.Models.Status;
using diga.web.Services.MangoPayServices;
using diga.web.Services.PlatformProcessingServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [Route("api/platform/payIns")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformPayInController : BaseController
    {
        private readonly IPlatformProcessingService _platformProcessingService;

        public PlatformPayInController(IPlatformProcessingService platformProcessingService)
            : base(null, null)
        {
            _platformProcessingService = platformProcessingService;
        }

        [HttpPost]
        public async Task<PlatformPayInDto> Post(PlatformPayInAddDto request)
        {
            return await _platformProcessingService.ProcessPayIn(UserId, request);
        }

        [HttpPost("confirm")]
        public async Task<ResponseData> Confirm(PlatformPayInConfirmDto request)
        {
            return await _platformProcessingService.ConfirmPayIn(UserId, request);
        }
    }
}
