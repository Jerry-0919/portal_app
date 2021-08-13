namespace diga.web.Models.Admin
{
    public class AdminPlatformEstimateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public bool IsOrdered { get; set; }
        public string Status { get; set; }
        public int VersionNumber { get; set; }
    }
}
