using diga.web.Models.PlatformContractSigning;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractSigningServices
{
    public interface IPlatformContractSigningService
    {
        Task<PlatformContractSigningDto> Get(int contractId);
        Task<ResponseData> Edit(int userId, int contractId, PlatformContractSigningDto request);
        Task<ResponseData> Approve(int userId, int contractId);
    }
}
