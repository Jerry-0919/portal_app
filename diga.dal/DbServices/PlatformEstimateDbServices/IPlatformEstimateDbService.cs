using diga.bl.Models.Platform.Estimates;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformEstimateDbServices
{
    public interface IPlatformEstimateDbService : IDefaultDbService<int, PlatformEstimate>
    {
        Task<PlatformEstimate> GetFull(int estimateId);
        Task<PlatformEstimate> GetWithSections(int estimateId);
        Task<List<PlatformEstimate>> GetFull(List<int> estimateId);
        Task<PlatformEstimate> GetApproval(int estimateId);
        Task<PlatformEstimate> Get(int contractId, int userId);
        Task<PlatformEstimate> GetActual(int contractId);
        Task<PlatformEstimate> GetApprove(int contractId);
        Task<PlatformEstimate> GetActualByEstimateId(int estimateId);
        Task<PlatformEstimate> GetMyBidEstimate(int userId, int contractId);
        Task<List<int>> GetExecutorVersionIds(int contractId);
        Task<List<PlatformEstimate>> List(List<int> estimateIds);
        Task<List<PlatformEstimate>> ExecutorsList(int contractId);
    }
}
