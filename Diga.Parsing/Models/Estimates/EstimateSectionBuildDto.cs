namespace Diga.Parsing.Models.Estimates
{
    public class EstimateSectionBuildDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }

        public int? ParentSectionId { get; set; }
    }
}
