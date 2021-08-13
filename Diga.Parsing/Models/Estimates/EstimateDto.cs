using System.Collections.Generic;

namespace Diga.Parsing.Models.Estimates
{
    public class EstimateDto
    {
        public string FileName { get; set; }

        public List<EstimateSectionDto> Sections { get; set; } = new List<EstimateSectionDto> { };
        public List<EstimatePositionDto> Positions { get; set; } = new List<EstimatePositionDto> { };
    }
}
