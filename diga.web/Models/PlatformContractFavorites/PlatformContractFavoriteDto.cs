using System;
using System.Collections.Generic;

namespace diga.web.Models.PlatformContractFavorites
{
    public class PlatformContractFavoriteDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string EditStatus { get; set; }
        public int Version { get; set; }
        public string VersionStatus { get; set; }
        public List<string> Categories { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? PlannedBuildEnd { get; set; }
    }
}
