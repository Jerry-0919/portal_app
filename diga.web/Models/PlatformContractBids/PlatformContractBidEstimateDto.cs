using System.Collections.Generic;

namespace diga.web.Models.PlatformContractBids
{
    public class PlatformContractBidEstimateDto
    {
        public List<PlatformContractBidEstimateSectionDto> Sections { get; set; }
        public List<PlatformContractBidEstimatePositionDto> Positions { get; set; }
    }
}
