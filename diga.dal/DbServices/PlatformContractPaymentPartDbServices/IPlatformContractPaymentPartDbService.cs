using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractPaymentPartDbServices
{
    public interface IPlatformContractPaymentPartDbService : IDefaultDbService<int, PlatformContractPaymentPart>
    {
        Task<List<PlatformContractPaymentPart>> List(int contractId);
    }
}
