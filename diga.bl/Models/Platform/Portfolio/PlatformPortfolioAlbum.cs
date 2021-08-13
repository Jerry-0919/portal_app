using diga.bl.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace diga.bl.Models.Platform.Portfolio
{
    public class PlatformPortfolioAlbum : IDbServiceEntity<int>
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string Name { get; set; }
        public DateTime PublishDate { get; set; }
        public string Description { get; set; }

        public IEnumerable<PlatformPortfolioPhoto> PortfolioPhotos { get; set; }
        public IEnumerable<PlatformAlbumCategory> AlbumCategories { get; set; }

        public PlatformPortfolioAlbum()
        {
            PortfolioPhotos = new List<PlatformPortfolioPhoto> { };
            AlbumCategories = new List<PlatformAlbumCategory> { };
        }
    }
}
