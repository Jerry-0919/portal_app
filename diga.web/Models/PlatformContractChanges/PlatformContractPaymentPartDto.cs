using System;

namespace diga.web.Models.PlatformContractChanges
{
    public class PlatformContractPaymentPartDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public double Value { get; set; }
        public double Percent { get; set; }
        public double? PercentOfWork { get; set; }
        public DateTime DateTime { get; set; }

        public bool? IsPaid { get; set; }
        public bool? IsMade { get; set; }

        public string InvoiceFile { get; set; }
        public string InvoiceFileName { get; set; }

        public string ProofFile { get; set; }
        public string ProofFileName { get; set; }
    }
}
