using System.Collections.Generic;

namespace diga.web.Models.PlatformContractBids
{
    public class PlatformContractBidListDto
    {
        public List<PlatformContractBidDto> Bids { get; set; }
        public PlatformContractBidDto UserBid { get; set; }
    }
}
