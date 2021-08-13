using System;
using System.Collections.Generic;

namespace diga.web.Models.Admin
{
    public class AdminPlatformContractFullDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public bool AddressHidden { get; set; }

        public AdminUserDto User { get; set; }
        public AdminPlatformContractTypeDto ContractType { get; set; }
        public AdminPlatformContractPriorityDto ContractPriority { get; set; }
        public AdminPlatformBalanceDto Balance { get; set; }
        public AdminPlatformCityDto City { get; set; }

        public double? Price { get; set; }

        public DateTime? PublishDate { get; set; }
        public DateTime? TenderEndDate { get; set; }

        public DateTime? BuildStart { get; set; }
        public DateTime? PlannedBuildEnd { get; set; }

        public string Status { get; set; }

        public AdminPlatformEstimateDto Estimate { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Files { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
