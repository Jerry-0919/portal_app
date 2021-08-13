using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Models.PlatformContracts
{
    public class PlatformContractExecutionDto
    {
        public double Works { get; set; }
        public double WorksLeft { get; set; }
        public double PaidPercent { get; set; }
        public double PaidMoney { get; set; }
        public double PaidPercentLeft { get; set; }
        public double PaidMoneyLeft { get; set; }
        public double Invoiced { get; set; }
        public double InvoicedLeft { get; set; }

        public double Term { get; set; }
        public double TermLeft { get; set; }
    }
}
