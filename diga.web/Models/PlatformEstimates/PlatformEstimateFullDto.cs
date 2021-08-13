using System.Collections.Generic;

namespace diga.web.Models.PlatformEstimates
{
    public class PlatformEstimateFullDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int? PlatformVATId { get; set; }
        public string VatType { get; set; }
        public double AnotherPercent { get; set; }
        public List<PlatformEstimateSectionDto> Sections { get; set; }
        public List<PlatformEstimateVatDto> PlatformEstimateVats { get; set; }
    }
}
