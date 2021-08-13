using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Notifications
{
    public class PlatformNotificationData
    {
        [Key]
        public int Id { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }

        [ForeignKey("PlatformNotification")]
        public int PlatformNotificationId { get; set; }
        public PlatformNotification PlatformNotification { get; set; }
    }
}
