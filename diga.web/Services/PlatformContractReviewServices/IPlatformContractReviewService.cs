using diga.web.Models.Pagination;
using diga.web.Models.PlatformContractReviews;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractReviewServices
{
    public interface IPlatformContractReviewService
    {
        Task<PaginatedList<PlatformContractReviewDto>> AllList(int userId, int skip, int take);
        Task<PaginatedList<PlatformContractReviewDto>> CustomerList(int userId, int skip, int take, int categoryId);
        Task<PaginatedList<PlatformContractReviewDto>> ExecutorList(int userId, int skip, int take, int categoryId);
        Task<ResponseData> Add(int userId, int contractId, PlatformContractReviewAddDto request);
        Task<PlatformContractReviewDto> GetExecutor(int contractId);
        Task<PlatformContractReviewDto> GetCustomer(int contractId);
    }
}