using System;
using System.Collections.Generic;

namespace diga.web.Models.PlatformPortfolioAlbums
{
    public class PlatformPortfolioAlbumDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public List<PlatformPortfolioAlbumCategoryDto> Categories { get; set; }
        public List<string> Photoes { get; set; }
    }
}
