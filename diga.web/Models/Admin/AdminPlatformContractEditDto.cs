using System;

namespace diga.web.Models.Admin
{
    public class AdminPlatformContractEditDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public bool AddressHidden { get; set; }

        public int UserId { get; set; }
        public int? ContractTypeId { get; set; }
        public int? ContractPriorityId { get; set; }
        public int? BalanceId { get; set; }
        public int? CityId { get; set; }

        public double? Price { get; set; }

        public DateTime? PublishDate { get; set; }
        public DateTime? TenderEndDate { get; set; }

        public DateTime? BuildStart { get; set; }
        public DateTime? PlannedBuildEnd { get; set; }

        public string Status { get; set; }

        public AdminPlatformEstimateDto Estimate { get; set; }
    }
}
