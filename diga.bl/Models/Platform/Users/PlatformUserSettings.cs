using diga.bl.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Users
{
    public class PlatformUserSettings : IDbServiceEntity<int>
    {
        [Key, ForeignKey("ApplicationUser")]
        public int Id { get; set; }

        public bool FilterHideMyBids { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public List<PlatformUserFilterCategory> FilterCategories { get; set; }
    }
}
