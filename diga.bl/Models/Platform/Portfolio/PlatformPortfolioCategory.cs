using diga.bl.Models.Platform.Information;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Portfolio
{
    public class PlatformAlbumCategory
    {
        [ForeignKey("PortfolioAlbum")]
        public int PortfolioAlbumId { get; set; }
        public PlatformPortfolioAlbum PortfolioAlbum { get; set; }

        [ForeignKey("PlatformCategory")]
        public int PlatformCategoryId { get; set; }
        public PlatformCategory PlatformCategory { get; set; }
    }
}
