using System.Collections.Generic;

namespace diga.web.Models.Admin
{
    public class AdminUserFullDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Domain { get; set; }
        public string Language { get; set; }
        public double? Balance { get; set; }

        public List<AdminUserVerificationDto> Verifications { get; set; }
    }
}
