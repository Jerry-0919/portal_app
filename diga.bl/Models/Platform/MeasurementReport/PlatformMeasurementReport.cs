using diga.bl.Interfaces;
using diga.bl.Models.Platform.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.MeasurementReport
{
    public class PlatformMeasurementReport : IDbServiceEntity<int>
    {
        [Key]
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

        [ForeignKey("PlatformContract")]
        public int PlatformContractId { get; set; }
        public PlatformContract PlatformContract { get; set; }
    }
}
