using System.Collections.Generic;

namespace diga.web.Models.PlatformContracts
{
    public class PlatformContractEstimateDto
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public bool IsOrdered { get; set; }
        public List<string> Files { get; set; }
    }
}
