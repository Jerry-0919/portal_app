using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Models.PlatformPayIns
{
    public class PlatformPayInConfirmDto
    {
        public string TransactionId { get; set; }
        public string Type { get; set; }
        public int ContractId { get; set; }
        public int Id { get; set; }
    }
}
