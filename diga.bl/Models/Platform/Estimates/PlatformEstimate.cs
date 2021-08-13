using diga.bl.Interfaces;
using diga.bl.Models.Platform.Contracts;
using diga.bl.Models.Platform.Information;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Estimates
{
    public class PlatformEstimate : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string FileName { get; set; }
        public bool IsOrdered { get; set; }

        [ForeignKey("Original")]
        public int? OriginalId { get; set; }
        public PlatformEstimate Original { get; set; }

        public int VersionNumber { get; set; }

        public double AnotherPercent { get; set; }
        public string VatType { get; set; }

        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }

        [ForeignKey("PlatformContract")]
        public int PlatformContractId { get; set; }

        [InverseProperty("PlatformEstimates")]
        public PlatformContract PlatformContract { get; set; }
        

        public List<PlatformEstimateSection> Sections { get; set; }
        public virtual ICollection<PlatformEstimateVAT> PlatformEstimateVATs { get; set; }

        public PlatformEstimate Clone()
        {
            PlatformEstimate clone = (PlatformEstimate)MemberwiseClone();

            clone.Original = null;
            clone.Creator = null;
            clone.PlatformContract = null;
            clone.Sections = new List<PlatformEstimateSection> { };

            return clone;
        }
    }
}
