using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Models.PlatformEstimates
{
    public class PlatformEstimateVatDto
    {
        public int PlatformEstimateId { get; set; }
        public int PlatformVATId { get; set; }
        public double Percent { get; set; }
    }
}
