using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.ManyToManyDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractBidDbServices
{
    public interface IPlatformContractBidDbService : IManyToManyDbService<PlatformContractBid>
    {
        Task<bool> Any(int userId, int contractId);
        Task<PlatformContractBid> Get(int userId, int contractId);
        Task<PlatformContractBid> GetFull(int userId, int contractId);
        Task<List<PlatformContractBid>> List(int contractId);
        Task<PlatformContractBid> GetAccepted(int contractId);
    }
}