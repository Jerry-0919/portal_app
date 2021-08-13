using diga.web.Models.Pagination;
using diga.web.Models.PlatformContractFavorites;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractFavoriteServices
{
    public interface IPlatformContractFavoriteService
    {
        Task<PaginatedList<PlatformContractFavoriteDto>> GetPaginatedList(int userId, int skip, int take);
        Task<ResponseData> Add(int userId, int contractId);
        Task<ResponseData> Remove(int userId, int contractId);
    }
}