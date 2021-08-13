using System.Collections.Generic;
using diga.web.Models.Texts;

namespace diga.web.Models.Admin
{
    public class AdminPlatformCategoryDto
    {
        public int Id { get; set; }
        public string NameId { get; set; }
        public double? ReservationPercent { get; set; }
        public int? ParentCategoryId { get; set; }
        public List<AdminPlatformCategoryDto> Children { get; set; }
    }
}
