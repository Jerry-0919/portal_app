using diga.bl.Models.Platform.Portfolio;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformPortfolioVideoDbServices
{
    public class PlatformPortfolioVideoDbService : DefaultDbService<int, PlatformPortfolioVideo>, IPlatformPortfolioVideoDbService
    {
        private readonly DigaContext _db;

        public PlatformPortfolioVideoDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<int> Count(int userId)
        {
            return await _db.PlatformPortfolioVideos.CountAsync(v => v.ApplicationUserId == userId);
        }

        public async Task<List<PlatformPortfolioVideo>> List(int userId, int skip, int take)
        {
            return await _db.PlatformPortfolioVideos.Where(v => v.ApplicationUserId == userId).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<List<PlatformPortfolioVideo>> List(int userId)
        {
            return await _db.PlatformPortfolioVideos.Where(v => v.ApplicationUserId == userId).ToListAsync();
        }
    }
}
