using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractChangeDbServices
{
    public interface IPlatformContractChangeDbService : IDefaultDbService<int, PlatformContractChange>
    {
        Task<List<PlatformContractChange>> List(int contractId);
        Task<PlatformContractChange> GetLast(int contractId);
    }
}
