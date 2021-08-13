using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.PlatformUserVerificationDbServices;
using diga.web.Models.Admin;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.AdminServices.AdminPlatformVerificationServices
{
    public class AdminPlatformVerificationService : IAdminPlatformVerificationService
    {
        private readonly IPlatformUserVerificationDbService _platformUserVerificationDbService;

        public AdminPlatformVerificationService(IPlatformUserVerificationDbService platformUserVerificationDbService)
        {
            _platformUserVerificationDbService = platformUserVerificationDbService;
        }

        public async Task<ResponseData> Check(AdminUserVerificationCheckDto request)
        {
            PlatformUserVerification verification = await _platformUserVerificationDbService.Get(request.VerificationId);

            verification.Status = request.Status;
            // Mail to do

            await _platformUserVerificationDbService.Update(verification);

            return ResponseData.Ok();
        }
    }
}
