using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Models.Auth0
{
    public class ManagementAccessDto
    {
        public string Access_token { get; set; }
        public double Expires_in { get; set; }
        public string Scope { get; set; }
        public string Token_type { get; set; }
    }
}
