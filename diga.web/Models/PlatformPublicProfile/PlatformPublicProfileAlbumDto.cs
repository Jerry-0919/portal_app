using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Models.PlatformPublicProfile
{
    public class PlatformPublicProfileAlbumDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public List<string> Photos { get; set; }
    }
}
