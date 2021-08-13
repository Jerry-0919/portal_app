using diga.bl.Models.Platform.Information;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Users
{
    public class PlatformUserFilterCategory
    {
        [ForeignKey("PlatformUserSettings")]
        public int PlatformUserSettingsId { get; set; }
        public PlatformUserSettings PlatformUserSettings { get; set; }

        [ForeignKey("PlatformCategory")]
        public int PlatformCategoryId { get; set; }
        public PlatformCategory PlatformCategory { get; set; }
    }
}
