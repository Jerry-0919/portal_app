using System;
using System.Collections.Generic;

namespace diga.web.Models.Users
{
    public class UserFullDto
    {
        public int Id { get; set; }

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
        public int UnreadNotificationsCount { get; set; }
        public int UnreadMessagesCount { get; set; }
        /// <summary>
        /// Erp domain
        /// </summary>
        public string Domain { get; set; }

        //mango pay
        public bool? IsPaymentUserCreated { get; set; }
        public bool? IsWalletCreated { get; set; }
        public bool? IsBankAccountConnected { get; set; }
        public string BankAccountValidationStatus { get; set; }

        public string VerificationStatus { get; set; }

        public List<UserPhoneNumberDto> PhoneNumbers { get; set; }
        public List<UserLanguageDto> Languages { get; set; }
    }
}
