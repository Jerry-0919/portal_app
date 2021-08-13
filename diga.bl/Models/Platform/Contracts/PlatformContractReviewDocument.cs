using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Contracts
{
    public class PlatformContractReviewDocument
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }

        [ForeignKey("PlatformContractReview")]
        public int PlatformContractReviewId { get; set; }
        public PlatformContractReview PlatformContractReview { get; set; }
    }
}
