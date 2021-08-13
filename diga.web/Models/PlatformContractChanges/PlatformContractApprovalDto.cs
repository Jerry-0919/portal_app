using System;
using System.Collections.Generic;

namespace diga.web.Models.PlatformContractChanges
{
    public class PlatformContractApprovalDto
    {
        public string ContractName { get; set; }
        public string EstimateName { get; set; }
        public string EstimateFileName { get; set; }
        public double? Price { get; set; }
        public string Description { get; set; }

        public DateTime? BuildStart { get; set; }
        public DateTime? PlannedBuildEnd { get; set; }
        public int? GeneralTerm { get; set; }

        public string PaymentType { get; set; }
        public string ComissionType { get; set; }
        public string CooperationType { get; set; }
        public double? BudgetOfWorks { get; set; }
        public double? HoursOfWorks { get; set; }
        public double? CostOfHour { get; set; }
        public string PaymentFrequency { get; set; }

        public bool? IsPrepayment { get; set; }
        public double? PrepaymentPercent { get; set; }
        public double? PrepaymentValue { get; set; }

        public List<PlatformContractPaymentPartDto> PaymentParts { get; set; }
    }
}
