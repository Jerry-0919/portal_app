using diga.bl.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Information
{
    public class PlatformCity : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public PlatformCountry Country { get; set; }
    }
}
