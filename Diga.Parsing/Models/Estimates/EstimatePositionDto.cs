namespace Diga.Parsing.Models.Estimates
{
    public class EstimatePositionDto
    {
        public string Number { get; set; }
        public string Description { get; set; }
        public string Measurement { get; set; }
        public double Quantity { get; set; }

        public EstimateSectionDto Section { get; set; }
    }
}
