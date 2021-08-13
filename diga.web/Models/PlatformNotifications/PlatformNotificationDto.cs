using System;
using System.Collections.Generic;

namespace diga.web.Models.PlatformNotifications
{
    public class PlatformNotificationDto
    {
        public int Id { get; set; }

        public bool IsRead { get; set; }
        public string Type { get; set; }
        public DateTime DateTime { get; set; }

        public List<PlatformNotificationDataDto> Datas { get; set; }
    }
}
