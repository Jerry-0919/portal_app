using diga.web.Models.PlatformUserSettings;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformUserSettingsServices
{
    public interface IPlatformUserSettingsService
    {
        Task<ResponseData> UpdateFilter(int userId, PlatformUserFilterSettingsDto settings);
        Task<PlatformUserFilterSettingsDto> GetFilter(int userId);
    }
}
