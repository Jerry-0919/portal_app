namespace diga.web.Models.PlatformEstimates
{
    public class PlatformEstimatePositionEditDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public double Quantity { get; set; }
        public string Measurement { get; set; }
        public string Description { get; set; }
        public int SectionId { get; set; }
        public double Price { get; set; }
        public string Notes { get; set; }
    }
}
