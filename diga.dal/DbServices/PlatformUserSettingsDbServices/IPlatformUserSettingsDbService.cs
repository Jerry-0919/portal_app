using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.DefaultDbServices;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformUserSettingsDbServices
{
    public interface IPlatformUserSettingsDbService : IDefaultDbService<int, PlatformUserSettings>
    {
        Task<PlatformUserSettings> GetFull(int userId);
    }
}
