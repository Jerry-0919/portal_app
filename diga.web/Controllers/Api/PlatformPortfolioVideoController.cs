using diga.web.Models.Pagination;
using diga.web.Models.PlatformPortfolioVideos;
using diga.web.Models.Status;
using diga.web.Services.PlatformPortfolioVideoServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/portfolio/videos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformPortfolioVideoController : BaseController
    {
        private readonly IPlatformPortfolioVideoService _platformPortfolioVideoService;

        public PlatformPortfolioVideoController(IPlatformPortfolioVideoService platformPortfolioVideoService)
            : base(null, null)
        {
            _platformPortfolioVideoService = platformPortfolioVideoService;
        }

        [HttpGet("{skip}/{take}")]
        public async Task<PaginatedList<PlatformPortfolioVideoDto>> List(int skip, int take)
        {
            return await _platformPortfolioVideoService.List(UserId, skip, take);
        }

        [HttpGet]
        public async Task<List<PlatformPortfolioVideoDto>> List()
        {
            return await _platformPortfolioVideoService.List(UserId);
        }

        [HttpPost]
        public async Task<ResponseData> Add(PlatformPortfolioVideoAddDto request)
        {
            return await _platformPortfolioVideoService.Add(UserId, request);
        }

        [HttpPut("{videoId}")]
        public async Task<ResponseData> Edit(int videoId, PlatformPortfolioVideoEditDto request)
        {
            return await _platformPortfolioVideoService.Edit(UserId, videoId, request);
        }
        
        [HttpDelete("{videoId}")]
        public async Task<ResponseData> Remove(int videoId)
        {
            return await _platformPortfolioVideoService.Remove(UserId, videoId);
        }
    }
}
