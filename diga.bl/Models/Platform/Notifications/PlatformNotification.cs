using diga.bl.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Notifications
{
    public class PlatformNotification : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public bool IsRead { get; set; }
        public DateTime DateTime { get; set; }
        public string Type { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public List<PlatformNotificationData> Datas { get; set; }
    }
}
