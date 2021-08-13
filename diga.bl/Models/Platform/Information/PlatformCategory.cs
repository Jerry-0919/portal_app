using diga.bl.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Information
{
    public class PlatformCategory : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public double? ReservationPercent { get; set; }

        [ForeignKey("Name")]
        public string NameId { get; set; }
        public Texts Name { get; set; }
        
        [ForeignKey("ParentCategory")]
        public int? ParentCategoryId { get; set; }
        public PlatformCategory ParentCategory { get; set; }
    }
}
