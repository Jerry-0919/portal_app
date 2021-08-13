using diga.bl.Models.Platform.Information;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Users
{
    public class PlatformUserCategory
    {
        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("PlatformCategory")]
        public int PlatformCategoryId { get; set; }
        public PlatformCategory PlatformCategory { get; set; }
    }
}
