using diga.bl.Enums.PlatformContracts;
using diga.bl.Models;
using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractDbServices
{
    public interface IPlatformContractDbService : IDefaultDbService<int, PlatformContract>
    {
        Task<bool> Any(int contractId, int userId);
        Task<PlatformContract> GetFull(int contractId);

        Task<int> Count(int userId, string filter, List<string> statuses);
        Task<int> CountFinished(int userId);
        Task<int> CountCurrent(int userId);
        Task<List<PlatformContract>> List(int userId, int skip, int take, string filter, List<string> statuses, SortType sort, bool isActual);

        Task<int> CountPublished(int userId, string filter, List<int> categories, List<string> tags, bool hideMyBidsProjects, int? balanceId);
        Task<List<PlatformContract>> ListPublished(int userId, int skip, int take, string filter,
            List<int> categories, List<string> tags, bool hideMyBidsProjects, int? balanceId, SortType sort);
        Task<List<PlatformContract>> List(int skip, int take);
        Task<int> GetLastVersion(int contractId);
        Task UpdateNotActual(int contractId);
        Task<int> CountVersions(int contractId);
        Task<List<PlatformContract>> ListVersions(int contractId, int skip, int take);

        Task<bool> IsCustomerOrExecutor(int contractId, int userId);
        Task<bool> IsCustomer(int contractId, int userId);
        Task<bool> IsExecutor(int contractId, int userId);
        Task<ApplicationUser> GetExecutor(int contractId);

        Task<PlatformContract> GetApproval(int contractId);
        Task<PlatformContract> GetWithEstimates(int contractId);
    }
}
