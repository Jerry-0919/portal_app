using diga.web.Models.Admin;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.AdminServices.AdminPlatformVerificationServices
{
    public interface IAdminPlatformVerificationService
    {
        Task<ResponseData> Check(AdminUserVerificationCheckDto request);
    }
}
