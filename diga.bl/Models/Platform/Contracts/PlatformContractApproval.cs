using diga.bl.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Contracts
{
    public class PlatformContractApproval : IDbServiceEntity<int>
    {
        [Key, ForeignKey("PlatformContract")]
        public int Id { get; set; }

        public bool ContractCustomer { get; set; }
        public bool ContractExecutor { get; set; }

        public bool EstimateCustomer { get; set; }
        public bool EstimateExecutor { get; set; }

        public bool SigningCustomer { get; set; }
        public bool SigningExecutor { get; set; }

        public bool FinishCustomer { get; set; }
        public bool FinishExecutor { get; set; }

        public string CustomContractText { get; set; }

        public PlatformContract PlatformContract { get; set; }
    }
}
