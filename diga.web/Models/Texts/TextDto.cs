namespace diga.web.Models.Texts
{
    public class TextDto
    {
        public string TextId { get; set; }
        public string En { get; set; }
        public string Ru { get; set; }
        public string Pt { get; set; }
        public string Es { get; set; }
        public bool? IsHtml { get; set; }
    }
}
