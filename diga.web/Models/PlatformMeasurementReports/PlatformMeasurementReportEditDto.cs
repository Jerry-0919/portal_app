using diga.web.Models.PlatformMeasurementReportPositions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Models.PlatformMeasurementReports
{
    public class PlatformMeasurementReportEditDto
    {
        public int ContractId { get; set; }
        public bool? IsPaid { get; set; }

        public string InvoiceFile { get; set; }
        public string InvoiceFileName { get; set; }

        public string ProofFile { get; set; }
        public string ProofFileName { get; set; }
        public string Status { get; set; }

        public List<PlatformMeasurementReportPositionEditDto> Positions { get; set; }
    }
}
