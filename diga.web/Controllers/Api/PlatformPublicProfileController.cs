using diga.web.Models.Pagination;
using diga.web.Models.PlatformContractReviews;
using diga.web.Models.PlatformPublicProfile;
using diga.web.Services.PlatformContractReviewServices;
using diga.web.Services.PlatformPublicProfile;
using diga.web.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [Route("api/platform/publicProfile")]
    [ApiController]
    public class PlatformPublicProfileController : ControllerBase
    {
        private readonly IPlatformPublicProfileService _platformPublicProfileService;
        private readonly IPlatformContractReviewService _platformContractReviewService;

        public PlatformPublicProfileController(IPlatformPublicProfileService platformPublicProfileService, IPlatformContractReviewService platformContractReviewService)
        {
            _platformPublicProfileService = platformPublicProfileService;
            _platformContractReviewService = platformContractReviewService;
        }

        [HttpGet("{userId}")]
        public async Task<PlatformPublicProfileDto> Get(int userId)
        {
            return await _platformPublicProfileService.Get(userId);
        }

        [HttpGet("reviews/{userId}/{skip}/{take}")]
        public async Task<PaginatedList<PlatformContractReviewDto>> Reviews(int userId, int skip, int take)
        {
            return await _platformContractReviewService.AllList(userId, skip, take);
        }
    }
}
