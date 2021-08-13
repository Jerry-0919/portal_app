using diga.bl.Models.Platform.Information;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Users
{
    public class PlatformUserLanguage
    {
        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("PlatformLanguage")]
        public int PlatformLanguageId { get; set; }
        public PlatformLanguage PlatformLanguage { get; set; }
    }
}
