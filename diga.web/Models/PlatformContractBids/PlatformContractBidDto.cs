namespace diga.web.Models.PlatformContractBids
{
    public class PlatformContractBidDto
    {
        public string FullName { get; set; }
        public string UserAvatar { get; set; }
        public int UserId { get; set; }
        public int UserRating { get; set; }
        public int UserBadReviews { get; set; }
        public int UserGoodReviews { get; set; }
        public int DaysNumber { get; set; }
        public double Price { get; set; }
        public string Status { get; set; }
        public string Text { get; set; }
        public bool Favorite { get; set; }
    }
}
