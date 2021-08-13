using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.DefaultDbServices;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformUserVerificationDbServices
{
    public interface IPlatformUserVerificationDbService : IDefaultDbService<int, PlatformUserVerification>
    {
        Task<PlatformUserVerification> GetLast(int userId);
    }
}
