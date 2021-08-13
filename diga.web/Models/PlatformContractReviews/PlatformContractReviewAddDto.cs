using System.Collections.Generic;

namespace diga.web.Models.PlatformContractReviews
{
    public class PlatformContractReviewAddDto
    {
        public string LikeText { get; set; }
        public string SuggestionText { get; set; }
        public List<PlatformContractReviewMarkDto> Marks { get; set; }
        public List<PlatformContractReviewDocumentDto> Documents { get; set; }
        public List<PlatformContractReviewImageDto> Images { get; set; }
    }
}
