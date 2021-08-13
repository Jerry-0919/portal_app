using diga.bl.Models;
using diga.bl.Models.Platform.Portfolio;
using diga.dal.DbServices.ManyToManyDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformAlbumCategoryDbServices
{
    public class PlatformAlbumCategoryDbService : ManyToManyDbService<PlatformAlbumCategory>, IPlatformAlbumCategoryDbService
    {
        private readonly DigaContext _db;

        public PlatformAlbumCategoryDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<List<PlatformAlbumCategory>> List(int albumId)
        {
            return await _db.PlatformAlbumCategories.Where(c => c.PortfolioAlbumId == albumId).ToListAsync();
        }
    }
}
