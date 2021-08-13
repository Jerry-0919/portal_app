using System.Collections.Generic;

namespace diga.web.Models.PlatformPortfolioAlbums
{
    public class PlatformPortfolioAlbumEditDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<string> Photos { get; set; }
    }
}
