using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Models.PlatformMeasurementReportPositions
{
    public class PlatformMeasurementReportPositionDto
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public double? QuantityCompleted { get; set; }
        public double? PercentCompleted { get; set; }
        public string Status { get; set; }
        public string RejectionReason { get; set; }
    }
}
