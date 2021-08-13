using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diga.web.Auth
{
    public class AuthOptions
    {
        public const string ISSUER = "diga"; 
        public const string AUDIENCE = "diga.pt"; 
        const string KEY = "38942c9b-6413-457d-ab6c-dc2f3a40f8d1";  
        public const int LIFETIME = 40000; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
