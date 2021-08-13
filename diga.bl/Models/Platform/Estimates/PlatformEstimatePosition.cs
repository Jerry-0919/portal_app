using diga.bl.Interfaces;
using diga.bl.Models.Platform.MeasurementReport;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Estimates
{
    public class PlatformEstimatePosition : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public string Description { get; set; }
        public string Measurement { get; set; }
        public double Quantity { get; set; }
        public int Number { get; set; }

        public double Price { get; set; }
        public string Notes { get; set; }

        [ForeignKey("EstimateSection")]
        public int EstimateSectionId { get; set; }
        public PlatformEstimateSection EstimateSection { get; set; }

        public List<PlatformMeasurementReportPosition> PlatformMeasurementReportPositions { get; set; }

        // For parsing
        [NotMapped]
        public string FullNumber { get; set; }

        public string GetFullNumber()
        {
            return $"{EstimateSection.GetFullNumber()}.{Number}";
        }

        public PlatformEstimatePosition Clone()
        {
            return (PlatformEstimatePosition)MemberwiseClone();
        }
    }
}
