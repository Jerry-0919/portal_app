using System.Collections.Generic;

namespace diga.web.Models.PlatformNotifications
{
    public class PlatformNotificationListDto
    {
        public List<PlatformNotificationDto> List { get; set; }
        public int CountAll { get; set; }
        public int CountUnread { get; set; }
    }
}
