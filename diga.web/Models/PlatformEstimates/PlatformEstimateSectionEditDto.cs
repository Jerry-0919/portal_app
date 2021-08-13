namespace diga.web.Models.PlatformEstimates
{
    public class PlatformEstimateSectionEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public int? ParentSectionId { get; set; }
        public string Notes { get; set; }
    }
}
