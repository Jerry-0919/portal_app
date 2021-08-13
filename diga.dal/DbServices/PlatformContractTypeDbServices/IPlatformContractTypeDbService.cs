using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.DefaultDbServices;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractTypeDbServices
{
    public interface IPlatformContractTypeDbService : IDefaultDbService<int, PlatformContractType>
    {
        Task<bool> Any(string name);
    }
}
