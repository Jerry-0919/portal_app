using diga.bl.Models.Platform.Contracts;
using diga.bl.Models.Platform.Estimates;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractCloneServices
{
    public interface IPlatformContractCloneService
    {
        Task<PlatformContract> Clone(int contractId);
        Task<PlatformEstimate> CloneEstimate(int estimateId, int creatorId, int versionNumber = 1);
    }
}
