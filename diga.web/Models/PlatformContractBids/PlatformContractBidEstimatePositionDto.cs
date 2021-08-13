namespace diga.web.Models.PlatformContractBids
{
    public class PlatformContractBidEstimatePositionDto
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public int Number { get; set; }
        public double Quantity { get; set; }
        public string Measurement { get; set; }
        public int EstimateSectionId { get; set; }
        public string Description { get; set; }
    }
}
