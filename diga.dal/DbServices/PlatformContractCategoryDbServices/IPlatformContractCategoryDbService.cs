using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.ManyToManyDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractDbServices
{
    public interface IPlatformContractCategoryDbService : IManyToManyDbService<PlatformContractCategory>
    {
        Task<List<PlatformContractCategory>> List(int contractId);
        Task AddRange(int contractId, List<int> categoryIds);
    }
}
