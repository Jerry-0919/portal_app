namespace Diga.Parsing.Models.Estimates
{
    public class EstimatePositionBuildDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public string Measurement { get; set; }
        public double Quantity { get; set; }
        public int SectionId { get; set; }
    }
}
