using diga.bl.Models.Platform.Portfolio;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformPortfolioPhotoDbServices
{
    public class PlatformPortfolioPhotoDbService : DefaultDbService<int, PlatformPortfolioPhoto>, IPlatformPortfolioPhotoDbService
    {
        private readonly DigaContext _db;

        public PlatformPortfolioPhotoDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<List<PlatformPortfolioPhoto>> List(int albumId)
        {
            return await _db.PlatformPortfolioPhotos.Where(p => p.PortfolioAlbumId == albumId).ToListAsync();
        }
    }
}
