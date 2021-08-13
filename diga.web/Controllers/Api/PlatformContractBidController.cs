using diga.web.Models.PlatformContractBids;
using diga.web.Models.Status;
using diga.web.Services.PlatformContractBidServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/contracts/bids")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformContractBidController : BaseController
    {
        private readonly IPlatformContractBidService _platformContractBidService;

        public PlatformContractBidController(IPlatformContractBidService platformContractBidService)
            : base(null, null)
        {
            _platformContractBidService = platformContractBidService;
        }

        [HttpPost("{contractId}")]
        public async Task<ResponseData> Add(int contractId, PlatformContractBidAddDto request)
        {
            return await _platformContractBidService.Add(UserId, contractId, request);
        }

        [HttpPut("{contractId}")]
        public async Task<ResponseData> Edit(int contractId, PlatformContractBidEditDto request)
        {
            return await _platformContractBidService.Edit(UserId, contractId, request);
        }

        [HttpPatch("{contractId}/withdrawn")]
        public async Task<ResponseData> Withdrawn(int contractId)
        {
            return await _platformContractBidService.Withdrawn(UserId, contractId);
        }

        [HttpPatch("{contractId}/withdrawn/return")]
        public async Task<ResponseData> WithdrawnReturn(int contractId)
        {
            return await _platformContractBidService.WithdrawnReturn(UserId, contractId);
        }

        [HttpPatch("{contractId}/remove/{bidUserId}")]
        public async Task<ResponseData> Remove(int contractId, int bidUserId)
        {
            return await _platformContractBidService.Remove(UserId, contractId, bidUserId);
        }

        [HttpPatch("{contractId}/remove/{bidUserId}/return")]
        public async Task<ResponseData> RemoveReturn(int contractId, int bidUserId)
        {
            return await _platformContractBidService.RemoveReturn(UserId, contractId, bidUserId);
        }

        [HttpGet("{contractId}/list")]
        public async Task<PlatformContractBidListDto> List(int contractId)
        {
            return await _platformContractBidService.List(UserId, contractId);
        }

        [HttpPost("{contractId}/favorite/{bidUserId}/add")]
        public async Task<ResponseData> AddFavorite(int contractId, int bidUserId)
        {
            return await _platformContractBidService.AddFavorite(UserId, contractId, bidUserId);
        }

        [HttpDelete("{contractId}/favorite/{bidUserId}/remove")]
        public async Task<ResponseData> RemoveFavorite(int contractId, int bidUserId)
        {
            return await _platformContractBidService.RemoveFavorite(UserId, contractId, bidUserId);
        }

        [HttpPatch("{contractId}/select/{bidUserId}")]
        public async Task<ResponseData> Select(int contractId, int bidUserId)
        {
            return await _platformContractBidService.Select(UserId, contractId, bidUserId);
        }
    }
}
