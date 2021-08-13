using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.ManyToManyDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractTagDbServices
{
    public interface IPlatformContractTagDbService : IManyToManyDbService<PlatformContractTag>
    {
        Task<List<PlatformContractTag>> List(int contractId);
    }
}