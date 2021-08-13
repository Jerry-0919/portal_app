using System.Collections.Generic;

namespace diga.web.Models.PlatformEstimates
{
    public class PlatformEstimateExecutorDto
    {
        public List<PlatformEstimateExecutorSectionDto> Sections { get; set; }
        public List<PlatformEstimateExecutorPositionDto> Positions { get; set; }
    }
}
