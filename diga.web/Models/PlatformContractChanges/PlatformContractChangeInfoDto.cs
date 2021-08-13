using System;

namespace diga.web.Models.PlatformContractChanges
{
    public class PlatformContractChangeInfoDto
    {
        public int GeneralTerm { get; set; }
        public DateTime? BuildStart { get; set; }
        public DateTime? PlannedBuildEnd { get; set; }
        public double? Price { get; set; }
        public string Description { get; set; }

        public string PaymentType { get; set; }
        public string ComissionType { get; set; }
        public string CooperationType { get; set; }
        public double? BudgetOfWorks { get; set; }
        public double? HoursOfWorks { get; set; }
        public double? CostOfHour { get; set; }
        public string PaymentFrequency { get; set; }
        
    }
}
