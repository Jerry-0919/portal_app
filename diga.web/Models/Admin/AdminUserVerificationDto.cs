using System;
using System.Collections.Generic;

namespace diga.web.Models.Admin
{
    public class AdminUserVerificationDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
        public List<string> Photos { get; set; }
    }
}
