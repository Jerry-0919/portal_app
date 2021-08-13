using diga.web.Models.PlatformContractDiscussions;
using diga.web.Services.PlatformContractDiscussionServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/notifications")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformContractDiscussionController : ControllerBase
    {
        private readonly IPlatformContractDiscussionService _platformContractDiscussionService;

        public PlatformContractDiscussionController(IPlatformContractDiscussionService platformContractDiscussionService)
        {
            _platformContractDiscussionService = platformContractDiscussionService;
        }

        //[HttpGet]
        //public async Task<List<PlatformContractDiscussionDto>>
    }
}
