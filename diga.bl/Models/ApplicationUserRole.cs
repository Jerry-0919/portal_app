using Microsoft.AspNetCore.Identity;

namespace diga.bl.Models
{
    public partial class ApplicationUserRole : IdentityUserRole<int>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}