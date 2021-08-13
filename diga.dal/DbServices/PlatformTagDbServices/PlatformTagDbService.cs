using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformTagDbServices
{
    public class PlatformTagDbService : DefaultDbService<int, PlatformTag>, IPlatformTagDbService
    {
        private readonly DigaContext _db;

        public PlatformTagDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<List<PlatformTag>> List(List<string> tags)
        {
            return await _db.PlatformTags.Where(t => tags.Contains(t.Name)).ToListAsync();
        }

        public async Task<bool> Any(string tag)
        {
            return await _db.PlatformTags.AnyAsync(t => t.Name == tag);
        }

        public async Task<PlatformTag> Get(string tag)
        {
            return await _db.PlatformTags.FirstOrDefaultAsync(t => t.Name == tag);
        }
    }
}
