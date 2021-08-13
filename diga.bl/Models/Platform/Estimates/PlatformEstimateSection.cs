using diga.bl.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Estimates
{
    public class PlatformEstimateSection : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int Number { get; set; }

        public string Notes { get; set; }

        [ForeignKey("Estimate")]
        public int EstimateId { get; set; }
        public PlatformEstimate Estimate { get; set; }

        [ForeignKey("ParentSection")]
        public int? ParentSectionId { get; set; }
        public PlatformEstimateSection ParentSection { get; set; }

        // For parsing
        [NotMapped]
        public string FullNumber { get; set; }

        public List<PlatformEstimatePosition> Positions { get; set; }

        public string GetFullNumber()
        {
            if (ParentSectionId.HasValue)
                return $"{ParentSection.GetFullNumber()}.{Number}";

            return Number.ToString();
        }

        public PlatformEstimateSection Clone()
        {
            return (PlatformEstimateSection)MemberwiseClone();
        }
    }
}
