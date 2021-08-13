using System;
using System.Collections.Generic;

namespace diga.web.Models.PlatformContractReviews
{
    public class PlatformContractReviewDto
    {
        public int Id { get; set; }

        public DateTime PublishDate { get; set; }

        public string ContractName { get; set; }
        public DateTime? BuildStart { get; set; }
        public DateTime? PlannedBuildEnd { get; set; }
        public double? Price { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }

        public string LikeText { get; set; }
        public string SuggestionText { get; set; }

        public List<PlatformContractReviewCategoryDto> PlatformContractReviewCategories { get; set; }
        public List<PlatformContractReviewMarkDto> Marks { get; set; }
        public List<PlatformContractReviewDocumentDto> Documents { get; set; }
        public List<PlatformContractReviewImageDto> Images { get; set; }
    }
}
