using diga.bl.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Contracts
{
    public class PlatformContractReview : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PlatformContract")]
        public int PlatformContractId { get; set; }
        public PlatformContract PlatformContract { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string LikeText { get; set; }
        public string SuggestionText { get; set; }
        public DateTime PublishDate { get; set; }

        public IEnumerable<PlatformContractReviewMark> Marks { get; set; }
        public IEnumerable<PlatformContractReviewDocument> Documents { get; set; }

        public PlatformContractReview()
        {
            Marks = new List<PlatformContractReviewMark> { };
        }
    }
}
