using diga.web.Models.PlatformNotifications;
using diga.web.Models.Status;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformNotificationServices
{
    public interface IPlatformNotificationService
    {
        Task<PlatformNotificationListDto> List(int userId, int skip, int take);
        Task AddNotification(int userId, PlatformNotificationDto notification);
        Task<ResponseData> Read(int notificationId);
    }
}
