using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.DefaultDbServices;

namespace diga.dal.DbServices.PlatformContractApprovalDbServices
{
    public interface IPlatformContractApprovalDbService : IDefaultDbService<int, PlatformContractApproval>
    {
    }
}
