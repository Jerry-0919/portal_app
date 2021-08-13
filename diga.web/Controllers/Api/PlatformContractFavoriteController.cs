using diga.web.Models.Pagination;
using diga.web.Models.PlatformContractFavorites;
using diga.web.Models.Status;
using diga.web.Services.PlatformContractFavoriteServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/favorites")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformContractFavoriteController : BaseController
    {
        private readonly IPlatformContractFavoriteService _platformContractFavoriteService;

        public PlatformContractFavoriteController(IPlatformContractFavoriteService platformContractFavoriteService)
            : base(null, null)
        {
            _platformContractFavoriteService = platformContractFavoriteService;
        }

        [HttpGet("{skip}/{take}")]
        public async Task<PaginatedList<PlatformContractFavoriteDto>> List(int skip, int take)
        {
            return await _platformContractFavoriteService.GetPaginatedList(UserId, skip, take);
        }

        [HttpPost("{contractId}")]
        public async Task<ResponseData> Add(int contractId)
        {
            return await _platformContractFavoriteService.Add(UserId, contractId);
        }

        [HttpDelete("{contractId}")]
        public async Task<ResponseData> Remove(int contractId)
        {
            return await _platformContractFavoriteService.Remove(UserId, contractId);
        }
    }
}
