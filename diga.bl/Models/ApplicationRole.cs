using diga.bl.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace diga.bl.Models
{
    public class ApplicationRole : IdentityRole<int>, IDbServiceEntity<int>
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
