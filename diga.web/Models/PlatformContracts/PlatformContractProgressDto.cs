namespace diga.web.Models.PlatformContracts
{
    public class PlatformContractProgressDto
    {
        public int ExecutorId { get; set; }
        public string LastChanged { get; set; }

        public bool ContractCustomer { get; set; }
        public bool ContractExecutor { get; set; }

        public bool EstimateCustomer { get; set; }
        public bool EstimateExecutor { get; set; }

        public bool SigningCustomer { get; set; }
        public bool SigningExecutor { get; set; }

        public bool FinishCustomer { get; set; }
        public bool FinishExecutor { get; set; }

        public string CustomContractText { get; set; }
    }
}
