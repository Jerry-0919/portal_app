using diga.bl.Models.Platform.Information;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Contracts
{
    public class PlatformContractCategory
    {
        [ForeignKey("PlatformContract")]
        public int PlatformContractId { get; set; }
        public PlatformContract PlatformContract { get; set; }

        [ForeignKey("PlatformCategory")]
        public int PlatformCategoryId { get; set; }
        public PlatformCategory PlatformCategory { get; set; }
    }
}
