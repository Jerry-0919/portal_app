using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractViewServices
{
    public interface IPlatformContractViewService
    {
        Task<ResponseData> Add(int userId, int contractId);
    }
}