using System.Collections.Generic;

namespace diga.web.Models.PlatformEstimates
{
    public class PlatformEstimateSectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string Notes { get; set; }

        public List<PlatformEstimateSectionDto> Sections { get; set; }
        public List<PlatformEstimatePositionDto> Positions { get; set; }
    }
}
