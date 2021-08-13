using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diga.web.Models.PlatformEstimates;

namespace diga.web.Models.PlatformMeasurementReports
{
    public class PlatformMeasurementReportDto
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public double PriceWithoutVat { get; set; }
        public double PriceWithVat { get; set; }

        public bool? IsPaid { get; set; }
        public bool? IsMade { get; set; }

        public string InvoiceFile { get; set; }
        public string InvoiceFileName { get; set; }

        public string ProofFile { get; set; }
        public string ProofFileName { get; set; }

        public PlatformEstimateFullDto Estimate { get; set; }
    }
}
