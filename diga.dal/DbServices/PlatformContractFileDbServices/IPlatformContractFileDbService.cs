using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractFileDbServices
{
    public interface IPlatformContractFileDbService : IDefaultDbService<int, PlatformContractFile>
    {
        Task<List<PlatformContractFile>> List(int contractId);
    }
}
