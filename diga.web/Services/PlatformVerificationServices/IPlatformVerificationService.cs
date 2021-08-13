using diga.web.Models.PlatformVerification;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformVerificationServices
{
    public interface IPlatformVerificationService
    {
        Task<ResponseData> Apply(PlatformVerificationApplyDto request, int userId);
    }
}
