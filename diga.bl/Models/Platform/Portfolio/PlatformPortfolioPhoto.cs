using diga.bl.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Portfolio
{
    public class PlatformPortfolioPhoto : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PlatformPortfolioAlbum")]
        public int PortfolioAlbumId { get; set; }
        public PlatformPortfolioAlbum PortfolioAlbum { get; set; }

        public string Value { get; set; }
    }
}
