using diga.web.Models.PlatformNotifications;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace diga.web.Hubs
{
    public interface IApplicationHubClient
    {
        [HubMethodName("add-notification")]
        Task AddNotification(PlatformNotificationDto notification);
    }
}
