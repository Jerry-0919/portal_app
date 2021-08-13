using System;

namespace diga.web.Models.Admin
{
    public class AdminPlatformUserRatingDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Points { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
    }
}
