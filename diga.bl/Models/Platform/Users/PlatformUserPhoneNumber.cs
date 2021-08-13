using diga.bl.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Users
{
    public class PlatformUserPhoneNumber : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string Type { get; set; }
        public string Value { get; set; }
    }
}
