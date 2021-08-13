using diga.bl.Models.Platform.Estimates;
using diga.web.Models.PlatformEstimates;
using diga.web.Models.Status;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformEstimateServices
{
    public interface IPlatformEstimateService
    {
        Task<PlatformEstimateFullDto> Get(int userId, int estimateId);
        Task<PlatformEstimateFullDto> GetMyBidEstimate(int userId, int contractId);
        Task<ResponseData> Edit(int userId, int estimateId, PlatformEstimateEditDto request);
        Task<ResponseData> EditMyBidEstimate(int userId, int contractId, PlatformEstimateExecutorDto request);
        Task<List<PlatformEstimateComparingDto>> ListExecutors(int contractId);
        Task<ResponseData> Approve(int userId, int contractId, int estimateId);
        Task<PlatformEstimateFullDto> GetApproval(int userId, int contractId);
        Task<ResponseData<List<PlatformEstimateVersionsDto>>> GetApprovalVersions(int userId, int contractId);
        Task<ResponseData> ApprovalEdit(int userId, int estimateId, PlatformEstimateEditDto request);
        Task<ResponseData> ApprovalCloneEdit(int userId, int estimateId, PlatformEstimateEditDto request);
        Task<int> ParseAdd(string name, string fileName, bool isOrdered, int contractId, int creatorId, int versionNumber);
        Task Remove(PlatformEstimate estimate);
        Task<ResponseData> ApprovalMain(int userId, int estimateId);
    }
}
