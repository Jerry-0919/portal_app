using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Models.PlatformPublicProfile
{
    public class PlatformPublicProfileDto
    {
        public int Id { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public int BadReviews { get; set; }
        public int GoodReviews { get; set; }
        public int Rating { get; set; }
        public string Position { get; set; }
        public string Resume { get; set; }
        public string Nationality { get; set; }
        public double AverageReview { get; set; }
        public int ContractsInWork { get; set; }
        public int ContractsCompleted { get; set; }
        public string LoadStatus { get; set; }
        public List<string> Categories { get; set; }
        public List<string> SubCategories { get; set; }
        public List<PlatformPublicProfileReviewDto> Reviews { get; set; }
        public PlatformPublicProfileCompanyDto Company { get; set; }

        public List<PlatformPublicProfileAlbumDto> PortfolioAlbums { get; set; }
        public List<PlatformPublicProfileVideoDto> PortfolioVideos { get; set; }
    }
}
