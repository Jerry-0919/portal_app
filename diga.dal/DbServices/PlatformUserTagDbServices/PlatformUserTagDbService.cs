using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.ManyToManyDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformUserTagDbServices
{
    public class PlatformUserTagDbService : ManyToManyDbService<PlatformUserTag>, IPlatformUserTagDbService
    {
        private readonly DigaContext _db;

        public PlatformUserTagDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<bool> Any(int userId, int tagId)
        {
            return await _db.PlatformUserTags.AnyAsync(t => t.ApplicationUserId == userId && t.PlatformTagId == tagId);
        }

        public async Task<bool> Any(int userId, string tag)
        {
            return await _db.PlatformUserTags.Include(t => t.PlatformTag)
                .AnyAsync(t => t.ApplicationUserId == userId && t.PlatformTag.Name == tag);
        }

        public async Task<int> Count(int userId)
        {
            return await _db.PlatformUserTags.CountAsync(t => t.ApplicationUserId == userId);
        }

        public async Task<PlatformUserTag> Get(int userId, int tagId)
        {
            return await _db.PlatformUserTags.FirstOrDefaultAsync(u => u.ApplicationUserId == userId && u.PlatformTagId == tagId);
        }

        public async Task<List<PlatformUserTag>> List(int userId)
        {
            return await _db.PlatformUserTags.Include(c => c.PlatformTag)
                .Where(c => c.ApplicationUserId == userId).ToListAsync();
        }
    }
}
