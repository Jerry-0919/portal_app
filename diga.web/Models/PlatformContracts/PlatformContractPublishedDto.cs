using System;

namespace diga.web.Models.PlatformContracts
{
    public class PlatformContractPublishedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? TenderEndDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public double? Price { get; set; }
        public int? BalanceId { get; set; }

        public string FullName { get; set; }
        public string UserAvatar { get; set; }
        public double Rating { get; set; }
        public int GoodReviews { get; set; }
        public int BadReviews { get; set; }
        public int CountDiscussions { get; set; }
        public int CountBids { get; set; }

        public int? EstimateId { get; set; }
        public string EstimateName { get; set; }
        public string EstimateFileName { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
