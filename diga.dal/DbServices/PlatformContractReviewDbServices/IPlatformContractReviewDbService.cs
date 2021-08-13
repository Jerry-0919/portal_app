using diga.bl.Models.Platform.Contracts;
using diga.bl.OutModels.PlatformContractReviews;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractReviewDbServices
{
    public interface IPlatformContractReviewDbService : IDefaultDbService<int, PlatformContractReview>
    {
        Task<int> CustomerCount(int userId, int categoryId);
        Task<List<PlatformContractReview>> CustomerList(int userId, int skip, int take, int categoryId);

        Task<int> ExecutorCount(int userId, int categoryId);
        Task<List<PlatformContractReview>> ExecutorList(int userId, int skip, int take, int categoryId);

        Task<List<PlatformContractReview>> AllList(int userId, int skip, int take);
        Task<int> AllCount(int userId);

        Task<List<PlatformContractReview>> List(int userId);
        Task<(int, int)> Count(int userId);
        Task<List<PlatformContractReviewUser>> Count(List<int> lists);

        Task<bool> Any(int contractId, int userId);
        Task<PlatformContractReview> Get(int contractId, int userId);

        Task<List<PlatformContractReview>> GetAll(int userId);
    }
}