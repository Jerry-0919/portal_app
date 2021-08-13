using diga.web.Models.PlatformEstimates;
using System.Collections.Generic;

namespace diga.web.Models.PlatformContractBids
{
    public class PlatformContractBidAddDto
    {
        public int DaysNumber { get; set; }
        public double Price { get; set; }
        public string Text { get; set; }
        public string Status { get; set; }

        public List<PlatformEstimateExecutorSectionDto> Sections { get; set; }
        public List<PlatformEstimateExecutorPositionDto> Positions { get; set; }
    }
}
