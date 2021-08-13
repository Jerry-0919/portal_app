using diga.bl.Models;
using System.Collections.Generic;
using System.Security.Claims;
namespace diga.web.Helpers
{
    public class ClaimsHelper
    {
        public ClaimsIdentity CommonClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                // new Claim(ClaimsIdentity.Role, user.Role.Name),
                new Claim("Id", user.Id.ToString()),
            };
            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole.Role.Name));
            }

            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
