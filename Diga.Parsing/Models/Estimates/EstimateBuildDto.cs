using System.Collections.Generic;

namespace Diga.Parsing.Models.Estimates
{
    public class EstimateBuildDto
    {
        public List<EstimateSectionBuildDto> Sections { get; set; } = new List<EstimateSectionBuildDto> { };
        public List<EstimatePositionBuildDto> Positions { get; set; } = new List<EstimatePositionBuildDto> { };
    }
}
