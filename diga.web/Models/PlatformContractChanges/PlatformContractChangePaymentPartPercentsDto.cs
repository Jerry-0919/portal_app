using System.Collections.Generic;

namespace diga.web.Models.PlatformContractChanges
{
    public class PlatformContractChangePaymentPartPercentsDto
    {
        public bool IsPrepayment { get; set; }
        public List<PlatformContractPaymentPartDto> Percents { get; set; }
    }
}
