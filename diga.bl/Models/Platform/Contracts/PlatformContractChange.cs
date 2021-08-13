using diga.bl.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Contracts
{
    public class PlatformContractChange : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PlatformContract")]
        public int PlatformContractId { get; set; }
        public PlatformContract PlatformContract { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string Case { get; set; }

        public string From { get; set; }
        public string To { get; set; }

        public DateTime DateTime { get; set; }

        public PlatformContractChange() { }

        public PlatformContractChange(int userId, int contractId, string caseValue, string from, string to, DateTime dateTime)
        {
            ApplicationUserId = userId;
            PlatformContractId = contractId;
            Case = caseValue;
            From = from;
            To = to;
            DateTime = dateTime;
        }
    }
}
