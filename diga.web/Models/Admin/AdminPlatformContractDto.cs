using System;

namespace diga.web.Models.Admin
{
    public class AdminPlatformContractDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public double? Price { get; set; }

        public DateTime? BuildStart { get; set; }
        public DateTime? PlannedBuildEnd { get; set; }
    }
}
