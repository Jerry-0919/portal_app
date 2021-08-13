using System;
using System.Collections.Generic;

namespace diga.web.Models.PlatformContracts
{
    public class PlatformContractContractorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContractType { get; set; }
        public bool IsFavorite { get; set; }

        public DateTime? BuildStart { get; set; }
        public DateTime? PlannedBuildEnd { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? TenderEndDate { get; set; }

        public int ViewsCount { get; set; }
        public string Description { get; set; }

        public string EstimateName { get; set; }
        public string EstimateFileName { get; set; }
        public List<string> Files { get; set; }

        public int UserId { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int? EstimateId { get; set; }
    }
}
