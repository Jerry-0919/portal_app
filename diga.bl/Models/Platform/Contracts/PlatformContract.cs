using diga.bl.Interfaces;
using diga.bl.Models.Platform.Chats;
using diga.bl.Models.Platform.Estimates;
using diga.bl.Models.Platform.Information;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Contracts
{
    public class PlatformContract : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public bool AddressHidden { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("ContractType")]
        public int? ContractTypeId { get; set; }
        public PlatformContractType ContractType { get; set; }

        [ForeignKey("ContractPriority")]
        public int? ContractPriorityId { get; set; }
        public PlatformContractPriority ContractPriority { get; set; }

        [ForeignKey("Balance")]
        public int? BalanceId { get; set; }
        public PlatformBalance Balance { get; set; }

        [ForeignKey("City")]
        public int? CityId { get; set; }
        public PlatformCity City { get; set; }

        public double? Price { get; set; } // null - если пользователь не указал цену

        public DateTime? PublishDate { get; set; }
        public DateTime? TenderEndDate { get; set; }

        public int? GeneralTerm { get; set; }
        public DateTime? BuildStart { get; set; }
        public DateTime? PlannedBuildEnd { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string Status { get; set; }
        public string EditStatus { get; set; }
        
        [ForeignKey("Original")]
        public int? OriginalId { get; set; }
        public PlatformContract Original { get; set; }

        public int VersionNumber { get; set; }
        public string VersionStatus { get; set; }

        public bool IsPrepayment { get; set; }
        public string RefusalCase { get; set; }
        public string ClosingCase { get; set; }

        /// <summary>
        /// PlatformContractPaymentTypes - вид расчета
        /// </summary>
        public string PaymentType { get; set; }
        /// <summary>
        /// PlatformContractComissionTypes - комиссия сейфа
        /// </summary>
        public string ComissionType { get; set; }
        public double? PrepaymentPercent { get; set; }
        public double? PrepaymentValue { get; set; }
        public string PrepaymentInvoiceFile { get; set; }
        public string PrepaymentInvoiceFileName { get; set; }
        /// <summary>
        /// Has customer paid the prepayment
        /// </summary>
        public bool? IsPrepaymentMade { get; set; }
        /// <summary>
        /// Was the prepayment paid to executor
        /// </summary>
        public bool? IsPrepaymentPaid { get; set; }
        /// <summary>
        /// PlatformContractCooperationType - вид сотрудничества
        /// </summary>
        public string CooperationType { get; set; }
        /// <summary>
        /// Бюджет на рабочих
        /// </summary>
        public double? BudgetOfWorks { get; set; }
        /// <summary>
        /// Планируемое количество часов
        /// </summary>
        public double? HoursOfWorks { get; set; }
        /// <summary>
        /// Стоимость в час
        /// </summary>
        public double? CostOfHour { get; set; }
        /// <summary>
        /// Частота платежей
        /// </summary>
        public string PaymentFrequency { get; set; }
        /// <summary>
        /// Выплачен ли резерв
        /// </summary>
        public bool? IsReservationPaid { get; set; }
        /// <summary>
        /// Выплатил ли заказчик сумму резерва
        /// </summary>
        public bool? IsResevationMade { get; set; }
        /// <summary>
        /// Главная смета
        /// </summary>
        public virtual int? MainEstimateId { get; set; }
        [ForeignKey("Id,MainEstimateId")]
        public virtual PlatformEstimate MainEstimate { get; set; }

        public PlatformContractApproval Approval { get; set; }

        public IEnumerable<PlatformEstimate> PlatformEstimates { get; set; }

        public IEnumerable<PlatformContractCategory> Categories { get; set; }
        public IEnumerable<PlatformContractTag> Tags { get; set; }
        public IEnumerable<PlatformContractFile> Files { get; set; }
        public IEnumerable<PlatformContractBid> Bids { get; set; }
        public IEnumerable<PlatformContractDiscussion> Discussions { get; set; }
        public IEnumerable<PlatformContractReview> Reviews { get; set; }
        public IEnumerable<PlatformContractPaymentPart> Parts { get; set; }
        public virtual ICollection<PlatformChatRoom> PlatformChatRooms { get; set; }

        public PlatformContract()
        {
            Categories = new List<PlatformContractCategory> { };
            Tags = new List<PlatformContractTag> { };
            Files = new List<PlatformContractFile> { };
            Bids = new List<PlatformContractBid> { };
            Discussions = new List<PlatformContractDiscussion> { };
            Reviews = new List<PlatformContractReview> { };
            Parts = new List<PlatformContractPaymentPart> { };

            Approval = new PlatformContractApproval { };
        }

        public PlatformContract Clone()
        {
            return (PlatformContract)MemberwiseClone();
        }
    }
}
