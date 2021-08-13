using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformUserRatingDbServices
{
    public class PlatformUserRatingDbService : DefaultDbService<int, PlatformUserRating>, IPlatformUserRatingDbService
    {
        private readonly DigaContext _db;

        public PlatformUserRatingDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<bool> Any(int userId, string description)
        {
            return await _db.PlatformUserRatings.AnyAsync(u => u.ApplicationUserId == userId && u.Description == description);
        }

        public async Task<int> CalculateRating(int userId)
        {
            return await _db.PlatformUserRatings.Where(r => r.ApplicationUserId == userId).SumAsync(r => r.Points);
        }

        public async Task<int> Count(int userId)
        {
            if (userId == 0)
                return await Count();

            return await _db.PlatformUserRatings.CountAsync(r => r.ApplicationUserId == userId);
        }

        public async Task<List<PlatformUserRating>> List(int skip, int take, int userId)
        {
            IQueryable<PlatformUserRating> query = _db.PlatformUserRatings.Include(r => r.ApplicationUser);

            if (userId != 0)
                query = query.Where(r => r.ApplicationUserId == userId);

            return await query.Skip(skip).Take(take).ToListAsync();
        }
    }
}
