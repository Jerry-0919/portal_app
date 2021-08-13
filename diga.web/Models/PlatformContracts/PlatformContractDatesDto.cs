using System;

namespace diga.web.Models.PlatformContracts
{
    public class PlatformContractDatesDto
    {
        public DateTime PublishDate { get; set; }
        public DateTime TenderEndDate { get; set; }
        public DateTime? BuildStart { get; set; }
        public DateTime? PlannedBuildEnd { get; set; }
    }
}
