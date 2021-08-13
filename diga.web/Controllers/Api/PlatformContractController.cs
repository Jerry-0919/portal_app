using diga.bl.Enums.PlatformContracts;
using diga.web.Models.Pagination;
using diga.web.Models.PlatformContracts;
using diga.web.Models.Status;
using diga.web.Services.PlatformContractServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/contracts")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformContractController : BaseController
    {
        private readonly IPlatformContractService _platformContractService;

        public PlatformContractController(IPlatformContractService platformContractService)
            : base(null, null)
        {
            _platformContractService = platformContractService;
        }

        [HttpPost]
        public async Task<PlatformContractAddResponseDto> Add()
        {
            return await _platformContractService.AddPlatformContract(UserId);
        }

        [HttpPatch("{contractId}/info")]
        public async Task<ResponseData> Info(int contractId, PlatformContractInfoDto request)
        {
            return await _platformContractService.UpdatePlatformContractInfo(UserId, contractId, request);
        }

        [HttpPatch("{contractId}/estimate")]
        public async Task<ResponseData> Estimate(int contractId, PlatformContractEstimateDto request)
        {
            return await _platformContractService.UpdatePlatformContractEstimate(UserId, contractId, request);
        }

        [HttpPatch("{contractId}/price")]
        public async Task<ResponseData> Price(int contractId, PlatformContractPriceDto request)
        {
            return await _platformContractService.UpdatePlatformContractPrice(UserId, contractId, request);
        }

        [HttpPatch("{contractId}/dates")]
        public async Task<ResponseData> Dates(int contractId, PlatformContractDatesDto request)
        {
            return await _platformContractService.UpdatePlatformContractDates(UserId, contractId, request);
        }

        [HttpPatch("{contractId}/complete")]
        public async Task<ResponseData> Complete(int contractId)
        {
            return await _platformContractService.CompletePlatformContract(UserId, contractId);
        }

        [HttpGet("{skip}/{take}")]
        public async Task<PaginatedList<PlatformContractDto>> List(int skip, int take,
            string filter = "", [FromQuery] List<string> statuses = null, SortType sort = SortType.DateDesc, bool isActual = false)
        {
            if (filter == null)
                filter = "";
            if (statuses == null)
                statuses = new List<string> { };

            return await _platformContractService.GetPaginatedList(UserId, skip, take, filter, statuses, sort, isActual);
        }

        [HttpGet("{contractId}")]
        public async Task<PlatformContractFullDto> Get(int contractId)
        {
            return await _platformContractService.Get(UserId, contractId);
        }

        [HttpGet("{contractId}/contractor")]
        public async Task<PlatformContractContractorDto> GetContractorDto(int contractId)
        {
            return await _platformContractService.GetContractorDto(UserId, contractId);
        }

        [HttpGet("{contractId}/progress")]
        public async Task<PlatformContractProgressDto> GetProgressDto(int contractId)
        {
            return await _platformContractService.GetExecutorDto(UserId, contractId);
        }

        [HttpGet("published/{skip}/{take}")]
        public async Task<PaginatedList<PlatformContractPublishedDto>> List(int skip, int take,
            bool hideMyBids = false, int? balanceId = null, string filter = "", SortType sort = SortType.DateAsc,
            [FromQuery] List<int> categories = null, [FromQuery] List<string> tags = null)
        {
            return await _platformContractService.GetPublished(hideMyBids, balanceId, filter, sort, categories, tags, UserId, skip, take);
        }

        [HttpDelete("{contractId}")]
        public async Task<ResponseData> Remove(int contractId)
        {
            return await _platformContractService.Remove(UserId, contractId);
        }

        [HttpPost("clone")]
        public async Task<ResponseData<PlatformContractCloneResponseDto>> Clone(PlatformContractCloneDto request)
        {
            return await _platformContractService.Clone(UserId, request);
        }

        [HttpGet("{contractId}/versions/{skip}/{take}")]
        public async Task<PaginatedList<PlatformContractDto>> Versions(int contractId, int skip, int take)
        {
            return await _platformContractService.GetPaginatedListVersions(UserId, contractId, skip, take);
        }

        [HttpPatch("{contractId}/tenderend")]
        public async Task<ResponseData> SetTenderEnd(int contractId, PlatformContractTenderEndRequestDto request)
        {
            return await _platformContractService.SetTenderEnd(UserId, contractId, request);
        }

        [HttpPatch("{contractId}/archive")]
        public async Task<ResponseData> Archive(int contractId)
        {
            return await _platformContractService.Archive(UserId, contractId);
        }

        [HttpPatch("{contractId}/refuse")]
        public async Task<ResponseData> Refuse(int contractId, PlatformContractRefuseRequestDto request)
        {
            return await _platformContractService.Refuse(UserId, contractId, request);
        }

        [HttpPatch("{contractId}/close")]
        public async Task<ResponseData> Close(int contractId, PlatformContractCloseRequestDto request)
        {
            return await _platformContractService.Close(UserId, contractId, request);
        }

        [HttpPatch("{contractId}/finish")]
        public async Task<ResponseData> Finish(int contractId)
        {
            return await _platformContractService.Finish(UserId, contractId);
        }

        [HttpGet("{contractId}/execution")]
        public async Task<PlatformContractExecutionDto> GetExecution(int contractId)
        {
            return await _platformContractService.GetExecution(UserId, contractId);
        }
    }
}
