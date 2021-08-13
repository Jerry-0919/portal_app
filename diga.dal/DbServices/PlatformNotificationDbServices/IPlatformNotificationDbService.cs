using diga.bl.Models.Platform.Notifications;
using diga.dal.DbServices.DefaultDbServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformNotificationDbServices
{
    public interface IPlatformNotificationDbService : IDefaultDbService<int, PlatformNotification>
    {
        Task<List<PlatformNotification>> List(int userId, int skip, int take);
        Task<int> Count(int userId);
        Task<int> CountUnread(int userId);
    }
}
