using diga.bl.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Contracts
{
    public class PlatformContractReviewMark : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Description")]
        public string DescriptionId { get; set; }
        public Texts Description { get; set; }

        public double Value { get; set; }

        [ForeignKey("Text")]
        public string TextId { get; set; }
        public Texts Text { get; set; }

        [ForeignKey("PlatformContractReview")]
        public int PlatformContractReviewId { get; set; }
        public PlatformContractReview PlatformContractReview { get; set; }
    }
}
