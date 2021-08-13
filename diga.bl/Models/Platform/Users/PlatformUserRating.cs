using diga.bl.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Users
{
    public class PlatformUserRating : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int Points { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
    }
}
