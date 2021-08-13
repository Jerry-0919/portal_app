using diga.bl.Enums.PlatformContracts;
using System.Collections.Generic;

namespace diga.web.Models.PlatformContracts
{
    public class PlatformContractPublishedRequestDto
    {
        public bool HideMyBidsProjects { get; set; }
        public int? BalanceId { get; set; }
        public string Filter { get; set; }
        public SortType Sort { get; set; } = SortType.DateAsc;
        public List<int> Categories { get; set; } = new List<int> { };
        public List<string> Tags { get; set; } = new List<string> { };
    }
}
