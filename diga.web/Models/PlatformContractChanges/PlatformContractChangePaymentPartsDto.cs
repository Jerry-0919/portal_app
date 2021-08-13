using System.Collections.Generic;

namespace diga.web.Models.PlatformContractChanges
{
    public class PlatformContractChangePaymentPartsDto
    {
        public bool IsPrepayment { get; set; }
        public double? PrepaymentPercent { get; set; }
        public double? PrepaymentValue { get; set; }
        public List<PlatformContractPaymentPartDto> Parts { get; set; }
    }
}
