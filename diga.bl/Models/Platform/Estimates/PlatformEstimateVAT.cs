using diga.bl.Models.Platform.Information;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace diga.bl.Models.Platform.Estimates
{
    public class PlatformEstimateVAT
    {
        [ForeignKey("PlatformEstimate")]
        public int PlatformEstimateId { get; set; }
        public PlatformEstimate PlatformEstimate { get; set; }

        [ForeignKey("PlatformVAT")]
        public int PlatformVATId { get; set; }
        public PlatformVAT PlatformVAT { get; set; }

        public double Percent { get; set; }
    }
}
