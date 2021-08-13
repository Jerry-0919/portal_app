using System.Collections.Generic;

namespace diga.web.Models.PlatformUserSettings
{
    public class PlatformUserFilterSettingsDto
    {
        public bool HideMyBids { get; set; } = false;
        public List<int> Categories { get; set; } = new List<int> { };
    }
}
