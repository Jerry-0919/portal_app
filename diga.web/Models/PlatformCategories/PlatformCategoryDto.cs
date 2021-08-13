using System.Collections.Generic;

namespace diga.web.Models.PlatformCategories
{
    public class PlatformCategoryDto
    {
        public int Id { get; set; }
        public string NameId { get; set; }
        public List<PlatformCategoryDto> PlatformCategories { get; set; }
    }
}
