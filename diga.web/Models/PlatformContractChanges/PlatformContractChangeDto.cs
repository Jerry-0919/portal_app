using System;

namespace diga.web.Models.PlatformContractChanges
{
    public class PlatformContractChangeDto
    {
        public string Case { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
