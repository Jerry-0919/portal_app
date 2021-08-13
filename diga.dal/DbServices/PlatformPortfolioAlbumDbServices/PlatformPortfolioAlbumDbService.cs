using diga.bl.Models.Platform.Portfolio;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformPortfolioAlbumDbServices
{
    public class PlatformPortfolioAlbumDbService : DefaultDbService<int, PlatformPortfolioAlbum>, IPlatformPortfolioAlbumDbService
    {
        private readonly DigaContext _db;

        public PlatformPortfolioAlbumDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<int> Count(int userId)
        {
            return await _db.PlatformPortfolioAlbums.CountAsync(a => a.ApplicationUserId == userId);
        }

        public async Task<List<PlatformPortfolioAlbum>> List(int userId, int skip, int take)
        {
            return await _db.PlatformPortfolioAlbums.Include(a => a.AlbumCategories).ThenInclude(c => c.PlatformCategory)
                .Include(a => a.PortfolioPhotos)
                .Where(a => a.ApplicationUserId == userId).Skip(skip).Take(take).ToListAsync();
        }
    }
}
