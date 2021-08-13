using diga.bl.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Users
{
    public class PlatformUserVerification : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public DateTime Created { get; set; }
        public string Status { get; set; }

        public ICollection<PlatformUserVerificationPhoto> Photos { get; set; }
    }
}
