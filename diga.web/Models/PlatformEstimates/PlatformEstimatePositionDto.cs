using diga.web.Models.PlatformMeasurementReportPositions;
using System.Collections.Generic;

namespace diga.web.Models.PlatformEstimates
{
    public class PlatformEstimatePositionDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string FullNumber { get; set; }
        public double? Quantity { get; set; }
        public string Measurement { get; set; }
        public string Description { get; set; }
        public double? Price { get; set; }
        public string Notes { get; set; }

        public List<PlatformMeasurementReportPositionDto> MeasurementReportPositions { get; set; }
    }
}
