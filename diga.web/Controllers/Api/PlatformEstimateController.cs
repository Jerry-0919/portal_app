using diga.web.Models.PlatformEstimates;
using diga.web.Models.Status;
using diga.web.Services.PlatformEstimateServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/estimates")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformEstimateController : BaseController
    {
        private readonly IPlatformEstimateService _platformEstimateService;

        public PlatformEstimateController(IPlatformEstimateService platformEstimateService)
            : base(null, null)
        {
            _platformEstimateService = platformEstimateService;
        }

        #region ConstructorForBid

        [HttpGet("{estimateId}")]
        public async Task<PlatformEstimateFullDto> Get(int estimateId)
        {
            return await _platformEstimateService.Get(UserId, estimateId);
        }

        [HttpPut("{estimateId}")]
        public async Task<ResponseData> Edit(int estimateId, PlatformEstimateEditDto request)
        {
            return await _platformEstimateService.Edit(UserId, estimateId, request);
        }

        [HttpGet("{contractId}/my")]
        public async Task<PlatformEstimateFullDto> GetMyBidEstimate(int contractId)
        {
            return await _platformEstimateService.GetMyBidEstimate(UserId, contractId);
        }

        [HttpPut("{contractId}/my")]
        public async Task<ResponseData> EditMyBidEstimate(int contractId, PlatformEstimateExecutorDto request)
        {
            return await _platformEstimateService.EditMyBidEstimate(UserId, contractId, request);
        }

        [HttpGet("{contractId}/executors")]
        public async Task<List<PlatformEstimateComparingDto>> ListEstimates(int contractId)
        {
            return await _platformEstimateService.ListExecutors(contractId);
        }

        #endregion

        #region Approval

        [HttpPost("{contractId}/{estimateId}/approve")]
        public async Task<ResponseData> Approve(int contractId, int estimateId)
        {
            return await _platformEstimateService.Approve(UserId, contractId, estimateId);
        }

        [HttpGet("{contractId}/approval")]
        public async Task<PlatformEstimateFullDto> Approval(int contractId)
        {
            return await _platformEstimateService.GetApproval(UserId, contractId);
        }

        [HttpPut("{estimateId}/approval")]
        public async Task<ResponseData> ApprovalEdit(int estimateId, PlatformEstimateEditDto request)
        {
            return await _platformEstimateService.ApprovalEdit(UserId, estimateId, request);
        }

        [HttpPut("{estimateId}/approval/clone")]
        public async Task<ResponseData> ApprovalCloneEdit(int estimateId, PlatformEstimateEditDto request)
        {
            return await _platformEstimateService.ApprovalCloneEdit(UserId, estimateId, request);
        }

        [HttpPatch("{estimateId}/approval/main")]
        public async Task<ResponseData> ApprovalMain(int estimateId)
        {
            return await _platformEstimateService.ApprovalMain(UserId, estimateId);
        }

        [HttpGet("{contractId}/approval/versions")]
        public async Task<ResponseData<List<PlatformEstimateVersionsDto>>> ApprovalVersions(int contractId)
        {
            return await _platformEstimateService.GetApprovalVersions(UserId, contractId);
        }

        #endregion
    }
}
