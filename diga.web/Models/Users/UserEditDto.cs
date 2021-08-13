using System;
using System.Collections.Generic;

namespace diga.web.Models.Users
{
    public class UserEditDto
    {
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Education { get; set; }
        public string PostalCode { get; set; }
        public int? NationalityId { get; set; }
        public int? ResidenceCountryId { get; set; }
        public string Language { get; set; }
        public string Resume { get; set; }
        public bool IsCompany { get; set; }
        public string MainPhoneNumber { get; set; }

        public List<UserPhoneNumberDto> PhoneNumbers { get; set; }
        public List<int> LanguageIds { get; set; }
    }
}
