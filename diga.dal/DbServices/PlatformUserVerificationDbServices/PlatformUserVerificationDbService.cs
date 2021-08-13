using diga.bl.Models.Platform.Users;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformUserVerificationDbServices
{
    public class PlatformUserVerificationDbService : DefaultDbService<int, PlatformUserVerification>, IPlatformUserVerificationDbService
    {
        private readonly DigaContext _db;

        public PlatformUserVerificationDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<PlatformUserVerification> GetLast(int userId)
        {
            return await _db.PlatformUserVerifications.FirstOrDefaultAsync(u => u.ApplicationUserId == userId
                && u.Created == _db.PlatformUserVerifications.Max(v => v.Created));
        }
    }
}
