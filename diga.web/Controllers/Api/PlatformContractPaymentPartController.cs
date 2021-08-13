using diga.web.Models.PlatformContractChanges;
using diga.web.Models.PlatformContractPaymentRequests;
using diga.web.Models.Status;
using diga.web.Services.PlatformContractPaymentPartServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [Route("api/platform/paymentParts")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PlatformContractPaymentPartController : BaseController
    {
        private readonly IPlatformContractPaymentPartService _platformContractPaymentPartService;
        public PlatformContractPaymentPartController(IPlatformContractPaymentPartService platformContractPaymentPartService)
            : base(null, null)
        {
            _platformContractPaymentPartService = platformContractPaymentPartService;
        }

        [HttpGet("{contractId}")]
        public async Task<List<PlatformContractPaymentPartDto>> List(int contractId)
        {
            return await _platformContractPaymentPartService.List(UserId, contractId);
        }

        [HttpPost("invoice/{contractId}/{id}")]
        public async Task<ResponseData> Invoice(int contractId, int id, PlatformContractPaymentRequestInvoiceEditDto request)
        {
            return await _platformContractPaymentPartService.EditInvoice(UserId, contractId, id, request);
        }

        [HttpPost("proof/{contractId}/{id}")]
        public async Task<ResponseData> Proof(int contractId, int id, PlatformContractPaymentRequestProofEditDto request)
        {
            return await _platformContractPaymentPartService.EditProof(UserId, contractId, id, request);
        }

        [HttpPost("paid/{contractId}/{id}")]
        public async Task<ResponseData> Invoice(int contractId, int id, PlatformContractPaymentRequestPaidEditDto request)
        {
            return await _platformContractPaymentPartService.EditPaid(UserId, contractId, id, request);
        }
    }
}
