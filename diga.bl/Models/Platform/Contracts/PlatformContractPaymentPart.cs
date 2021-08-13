using diga.bl.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Contracts
{
    public class PlatformContractPaymentPart : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PlatformContract")]
        public int PlatformContractId { get; set; }
        public PlatformContract PlatformContract { get; set; }

        public DateTime DateTime { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double Percent { get; set; }
        public double? PercentOfWork { get; set; }
        public int Number { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsMade { get; set; }

        public string InvoiceFile { get; set; }
        public string InvoiceFileName { get; set; }

        public string ProofFile { get; set; }
        public string ProofFileName { get; set; }
    }
}
