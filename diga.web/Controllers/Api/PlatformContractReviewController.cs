using diga.web.Models.Pagination;
using diga.web.Models.PlatformContractReviews;
using diga.web.Models.Status;
using diga.web.Services.PlatformContractReviewServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [ApiController]
    [Route("api/platform/contracts/reviews")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformContractReviewController : BaseController
    {
        private readonly IPlatformContractReviewService _platformContractReviewService;

        public PlatformContractReviewController(IPlatformContractReviewService platformContractReviewService)
            : base(null, null)
        {
            _platformContractReviewService = platformContractReviewService;
        }

        [HttpGet("customer/{skip}/{take}")]
        public async Task<PaginatedList<PlatformContractReviewDto>> CustomerList(int skip, int take, int categoryId = 0)
        {
            return await _platformContractReviewService.CustomerList(UserId, skip, take, categoryId);
        }

        [HttpGet("executor/{skip}/{take}")]
        public async Task<PaginatedList<PlatformContractReviewDto>> ExecutorList(int skip, int take, int categoryId = 0)
        {
            return await _platformContractReviewService.ExecutorList(UserId, skip, take, categoryId);
        }

        [HttpPost("{contractId}")]
        public async Task<ResponseData> Add(int contractId, PlatformContractReviewAddDto request)
        {
            return await _platformContractReviewService.Add(UserId, contractId, request);
        }

        [HttpGet("{contractId}/executor")]
        public async Task<PlatformContractReviewDto> GetExecutor(int contractId)
        {
            return await _platformContractReviewService.GetExecutor(contractId);
        }

        [HttpGet("{contractId}/customer")]
        public async Task<PlatformContractReviewDto> GetCustomer(int contractId)
        {
            return await _platformContractReviewService.GetCustomer(contractId);
        }
    }
}
