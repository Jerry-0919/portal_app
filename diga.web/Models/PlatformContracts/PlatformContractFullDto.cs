using System;
using System.Collections.Generic;

namespace diga.web.Models.PlatformContracts
{
    public class PlatformContractFullDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public bool AddressHidden { get; set; }

        public string ContractType { get; set; }
        public int? ContractTypeId { get; set; }

        public int? ContractPriorityId { get; set; }
        public int? CityId { get; set; }
        public int? CountryId { get; set; }

        public double? Price { get; set; }
        public int? BalanceId { get; set; }

        public DateTime? PublishDate { get; set; }
        public DateTime? TenderEndDate { get; set; }

        public DateTime? BuildStart { get; set; }
        public DateTime? PlannedBuildEnd { get; set; }

        public string Description { get; set; }
        public string Status { get; set; }
        public string EditStatus { get; set; }

        public string EstimateName { get; set; }
        public string EstimateFileName { get; set; }
        public bool? IsEstimateOrdered { get; set; }

        public bool? IsPrepayment { get; set; }
        public double? PrepaymentValue { get; set; }
        public bool? IsPrepaymentMade { get; set; }
        public bool? IsPrepaymentPaid { get; set; }

        public string PaymentType { get; set; }
        public string ComissionType { get; set; }
        public string CooperationType { get; set; }
        public double? BudgetOfWorks { get; set; }
        public double? HoursOfWorks { get; set; }
        public double? CostOfHour { get; set; }
        public string PaymentFrequency { get; set; }
        public bool? IsReservationPaid { get; set; }
        public bool? IsReservationMade { get; set; }
        public double? SumOfReservation { get; set; }

        /// <summary>
        /// Original estimate
        /// </summary>
        public int? EstimateId { get; set; }
        /// <summary>
        /// Main estimate
        /// </summary>
        public int? MainEstimateId { get; set; }

        public List<int> Categories { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Files { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
