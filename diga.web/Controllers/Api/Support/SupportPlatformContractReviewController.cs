using diga.web.Models.Status;
using diga.web.Services.SupportServices.SupportPlatformContractReviewServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api.Support
{
    [ApiController]
    [Route("api/support/platform/contracts/reviews")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Support")]
    public class SupportPlatformContractReviewController : ControllerBase
    {
        private readonly ISupportPlatformContractReviewService _platformContractService;

        public SupportPlatformContractReviewController(
            ISupportPlatformContractReviewService platformContractService)
        {
            _platformContractService = platformContractService;
        }

        [HttpDelete("{reviewId}")]
        public async Task<ResponseData> RemovePlatoformContract(int contractId)
        {
            return await _platformContractService.Remove(contractId);
        }
    }
}
