using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.DefaultDbServices;

namespace diga.dal.DbServices.PlatformContractApprovalDbServices
{
    public class PlatformContractApprovalDbService : DefaultDbService<int, PlatformContractApproval>, IPlatformContractApprovalDbService
    {
        public PlatformContractApprovalDbService(DigaContext db)
            : base(db)
        {

        }
    }
}
