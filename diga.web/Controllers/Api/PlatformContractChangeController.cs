using diga.web.Models.PlatformContractChanges;
using diga.web.Models.Status;
using diga.web.Services.PlatformContractChangeServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/contract/changes")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformContractChangeController : BaseController
    {
        private readonly IPlatformContractChangeService _platformContractChangeService;

        public PlatformContractChangeController(IPlatformContractChangeService platformContractChangeService)
            : base(null, null)
        {
            _platformContractChangeService = platformContractChangeService;
        }

        [HttpGet("{contractId}")]
        public async Task<Dictionary<DateTime, List<PlatformContractChangeDto>>> List(int contractId)
        {
            return await _platformContractChangeService.List(UserId, contractId);
        }

        [HttpGet("{contractId}/approval")]
        public async Task<ResponseData<PlatformContractApprovalDto>> Get(int contractId)
        {
            return await _platformContractChangeService.GetApproval(UserId, contractId);
        }

        [HttpPatch("{contractId}/approve")]
        public async Task<ResponseData> Approve(int contractId)
        {
            return await _platformContractChangeService.Approve(UserId, contractId);
        }

        [HttpPut("{contractId}")]
        public async Task<ResponseData> Edit(int contractId, PlatformContractChangeInfoDto request)
        {
            return await _platformContractChangeService.Edit(UserId, contractId, request);
        }

        [HttpPost("{contractId}/payment/parts")]
        public async Task<ResponseData> ChangePaymentParts(int contractId, PlatformContractChangePaymentPartsDto request)
        {
            return await _platformContractChangeService.EditPaymentParts(UserId, contractId, request);
        }

        //[HttpPost("{contractId}/payment/parts/percents")]
        //public async Task<ResponseData> ChangePaymentPartPercents(int contractId, PlatformContractChangePaymentPartPercentsDto request)
        //{
        //    return await _platformContractChangeService.EditPaymentPartPercents(UserId, contractId, request);
        //}
    }
}
