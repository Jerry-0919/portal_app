using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.ManyToManyDbServices;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractViewDbServices
{
    public interface IPlatformContractViewDbService : IManyToManyDbService<PlatformContractView>
    {
        Task<int> Count(int contractId);
        Task<bool> Any(int userId, int contractId);
    }
}