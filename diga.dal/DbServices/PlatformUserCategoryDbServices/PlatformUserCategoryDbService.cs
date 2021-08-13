using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.ManyToManyDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformUserCategoryDbServices
{
    public class PlatformUserCategoryDbService : ManyToManyDbService<PlatformUserCategory>, IPlatformUserCategoryDbService
    {
        private readonly DigaContext _db;

        public PlatformUserCategoryDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<bool> Any(int userId, int categoryId)
        {
            return await _db.PlatformUserCategories.AnyAsync(c => c.ApplicationUserId == userId && c.PlatformCategoryId == categoryId);
        }

        public async Task<PlatformUserCategory> Get(int userId, int categoryId)
        {
            return await _db.PlatformUserCategories.FirstOrDefaultAsync(c => c.ApplicationUserId == userId && c.PlatformCategoryId == categoryId);
        }

        public async Task<List<PlatformUserCategory>> List(int userId)
        {
            return await _db.PlatformUserCategories.Include(c => c.PlatformCategory)
                .Where(c => c.ApplicationUserId == userId).ToListAsync();
        }
    }
}
