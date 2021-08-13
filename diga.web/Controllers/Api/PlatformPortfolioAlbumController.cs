using diga.web.Models.Pagination;
using diga.web.Models.PlatformPortfolioAlbums;
using diga.web.Models.Status;
using diga.web.Services.PlatformPortfolioAlbumServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/portfolio/albums")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformPortfolioAlbumController : BaseController
    {
        private readonly IPlatformPortfolioAlbumService _platformPortfolioAlbumService;

        public PlatformPortfolioAlbumController(IPlatformPortfolioAlbumService platformPortfolioAlbumService)
            : base(null, null)
        {
            _platformPortfolioAlbumService = platformPortfolioAlbumService;
        }

        [HttpGet("{skip}/{take}")]
        public async Task<PaginatedList<PlatformPortfolioAlbumDto>> List(int skip, int take)
        {
            return await _platformPortfolioAlbumService.List(UserId, skip, take);
        }

        [HttpPost]
        public async Task<ResponseData> Add(PlatformPortfolioAlbumAddDto request)
        {
            return await _platformPortfolioAlbumService.Add(UserId, request);
        }

        [HttpPut("{albumId}")]
        public async Task<ResponseData> Edit(int albumId, PlatformPortfolioAlbumEditDto request)
        {
            return await _platformPortfolioAlbumService.Edit(UserId, albumId, request);
        }

        [HttpDelete("{albumId}")]
        public async Task<ResponseData> Remove(int albumId)
        {
            return await _platformPortfolioAlbumService.Remove(UserId, albumId);
        }
    }
}
