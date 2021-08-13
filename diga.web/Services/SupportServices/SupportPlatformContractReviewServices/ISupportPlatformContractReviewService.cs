using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.SupportServices.SupportPlatformContractReviewServices
{
    public interface ISupportPlatformContractReviewService
    {
        Task<ResponseData> Remove(int reviewId);
    }
}
