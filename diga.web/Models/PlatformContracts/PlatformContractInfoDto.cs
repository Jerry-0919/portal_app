using System.Collections.Generic;

namespace diga.web.Models.PlatformContracts
{
    public class PlatformContractInfoDto
    {
        public string Name { get; set; }
        public int? PriorityId { get; set; }
        public int? ContractTypeId { get; set; }
        public int? CityId { get; set; }
        public string Address { get; set; }
        public bool AddressHidden { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int> { };
        public List<string> Tags { get; set; } = new List<string> { };
    }
}
