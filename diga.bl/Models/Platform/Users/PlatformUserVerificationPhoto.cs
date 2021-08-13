using diga.bl.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Users
{
    public class PlatformUserVerificationPhoto : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PlatformUserVerification")]
        public int PlatformUserVerificationId { get; set; }
        public PlatformUserVerification PlatformUserVerification { get; set; }

        public string Value { get; set; }
    }
}
