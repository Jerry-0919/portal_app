namespace Diga.Parsing.Models.Estimates
{
    public class EstimateSectionDto
    {
        public string Name { get; set; }
        public string Number { get; set; }

        public EstimateSectionDto ParentSection { get; set; }
    }
}
