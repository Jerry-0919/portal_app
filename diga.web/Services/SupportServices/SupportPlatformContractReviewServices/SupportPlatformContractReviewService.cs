using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.PlatformContractReviewDbServices;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.SupportServices.SupportPlatformContractReviewServices
{
    public class SupportPlatformContractReviewService : ISupportPlatformContractReviewService
    {
        private readonly IPlatformContractReviewDbService _platformContractReviewDbService;

        public SupportPlatformContractReviewService(IPlatformContractReviewDbService platformContractReviewDbService)
        {
            _platformContractReviewDbService = platformContractReviewDbService;
        }

        public async Task<ResponseData> Remove(int reviewId)
        {
            PlatformContractReview review = await _platformContractReviewDbService.Get(reviewId);
            await _platformContractReviewDbService.Remove(review);

            return ResponseData.Ok();
        }
    }
}
