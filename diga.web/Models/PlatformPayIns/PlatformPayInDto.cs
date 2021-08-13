using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Entities.GET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Models.PlatformPayIns
{
    public class PlatformPayInDto
    {
        public TransactionStatus Status { get; set; }
        public BankAccountIbanDTO BankAccount { get; set; }
        public string WireReference { get; set; }
        public string RedirectURL { get; set; }
    }
}
