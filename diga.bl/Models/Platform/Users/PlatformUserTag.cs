using diga.bl.Models.Platform.Information;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Users
{
    public class PlatformUserTag
    {
        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("PlatformTag")]
        public int PlatformTagId { get; set; }
        public PlatformTag PlatformTag { get; set; }
    }
}
