using diga.bl.Interfaces;
using diga.bl.Models.Platform.Contracts;
using diga.bl.Models.Platform.Information;
using diga.bl.Models.Platform.MangoPay;
using diga.bl.Models.Platform.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models
{
    public class ApplicationUser : IdentityUser<int>, IDbServiceEntity<int>
    {
        public string AuthToken { get; set; }

        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Domain { get; set; }
        public string Language { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Education { get; set; }
        public string PostalCode { get; set; }
        public string Resume { get; set; }
        public bool IsCompany { get; set; }

        public double? Balance { get; set; }

        public string PasswordCode { get; set; }
        public DateTime? LastSeen { get; set; }
        public bool NewMessagesEmailNotification { get; set; }
        public string MangoPayUserId { get; set; }
        public string MangoPayWalletId { get; set; }
        public string MangoPayKYCValidationStatus { get; set; }

        [ForeignKey("Nationality")]
        public int? NationalityId { get; set; }
        public PlatformCountry Nationality { get; set; }

        [ForeignKey("ResidenceCountry")]
        public int? ResidenceCountryId { get; set; }
        public PlatformCountry ResidenceCountry { get; set; }

        public PlatformCompany PlatformCompany { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public virtual ICollection<PlatformUserPhoneNumber> PhoneNumbers { get; set; }
        public virtual ICollection<PlatformUserLanguage> Languages { get; set; }
        public virtual ICollection<PlatformUserRating> Ratings { get; set; }
        public virtual ICollection<PlatformContractReview> Reviews { get; set; }
        public virtual ICollection<UserTariffs> UserTariffs { get; set; }
        public virtual ICollection<PlatformMangoPayUserBankAccount> PlatformMangoPayUserBankAccounts { get; set; }
        public virtual ICollection<PlatformUserVerification> PlatformUserVerifications { get; set; }
    }
}
