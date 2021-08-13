using System.Collections.Generic;

namespace diga.web.Models.PlatformPayServices
{
    public class BankAccountDto
    {
        public string Id { get; set; }
        public string OwnerName { get; set; }
        public string OwnerAddressObsolete { get; set; }
        public bool Active { get; set; }
    }
}
