using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformUserSettingsDbServices
{
    public class PlatformUserSettingsDbService : DefaultDbService<int, PlatformUserSettings>, IPlatformUserSettingsDbService
    {
        private readonly DigaContext _db;

        public PlatformUserSettingsDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<PlatformUserSettings> GetFull(int userId)
        {
            return await _db.PlatformUserSettings.Include(s => s.FilterCategories)
                .FirstOrDefaultAsync(s => s.Id == userId);
        }
    }
}
