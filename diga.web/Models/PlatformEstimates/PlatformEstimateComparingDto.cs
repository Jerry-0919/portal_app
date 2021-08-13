using System.Collections.Generic;

namespace diga.web.Models.PlatformEstimates
{
    public class PlatformEstimateComparingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? PlatformVATId { get; set; }
        public double AnotherPercent { get; set; }

        public string FullName { get; set; }
        public string UserAvatar { get; set; }
        public int? DaysNumber { get; set; }
        public double? Price { get; set; }

        public List<PlatformEstimatePositionDto> Positions { get; set; }
    }
}
