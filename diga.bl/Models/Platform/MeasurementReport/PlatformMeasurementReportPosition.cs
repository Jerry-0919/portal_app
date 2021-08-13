using diga.bl.Interfaces;
using diga.bl.Models.Platform.Contracts;
using diga.bl.Models.Platform.Estimates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace diga.bl.Models.Platform.MeasurementReport
{
    public class PlatformMeasurementReportPosition : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }
        public double? QuantityCompleted { get; set; }
        public double? PercentCompleted { get; set; }

        public string Status { get; set; }
        public string RejectionReason { get; set; }

        [ForeignKey("PlatformEstimatePosition")]
        public int PositionId { get; set; }
        public PlatformEstimatePosition PlatformEstimatePosition { get; set; }

        [ForeignKey("PlatformMeasurementReport")]
        public int ReportId { get; set; }
        public PlatformMeasurementReport PlatformMeasurementReport { get; set; }
    }
}
