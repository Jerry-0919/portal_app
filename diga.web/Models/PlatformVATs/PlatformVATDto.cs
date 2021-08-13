namespace diga.web.Models.PlatformVATs
{
    public class PlatformVATDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Percent { get; set; }
        public string RegionCode { get; set; }
        public int? CountryId { get; set; }
    }
}
