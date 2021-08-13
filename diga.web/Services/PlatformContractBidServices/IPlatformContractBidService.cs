using diga.web.Models.PlatformContractBids;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractBidServices
{
    public interface IPlatformContractBidService
    {
        Task<ResponseData> Add(int userId, int contractId, PlatformContractBidAddDto request);
        Task<ResponseData> Edit(int userId, int contractId, PlatformContractBidEditDto request);
        Task<ResponseData> Withdrawn(int userId, int contractId);
        Task<ResponseData> Remove(int userId, int contractId, int bidUserId);
        Task<ResponseData> WithdrawnReturn(int userId, int contractId);
        Task<ResponseData> RemoveReturn(int userId, int contractId, int bidUserId);
        Task<PlatformContractBidListDto> List(int userId, int contractId);
        Task<ResponseData> AddFavorite(int userId, int contractId, int bidUserId);
        Task<ResponseData> RemoveFavorite(int userId, int contractId, int bidUserId);
        Task<ResponseData> Select(int userId, int contractId, int bidUserId);
    }
}