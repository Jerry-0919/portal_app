using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.ManyToManyDbServices;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractViewDbServices
{
    public class PlatformContractViewDbService : ManyToManyDbService<PlatformContractView>, IPlatformContractViewDbService
    {
        private readonly DigaContext _db;

        public PlatformContractViewDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<bool> Any(int userId, int contractId)
        {
            return await _db.PlatformContractViews.AnyAsync(c => c.ApplicationUserId == userId && c.PlatformContractId == contractId);
        }

        public async Task<int> Count(int contractId)
        {
            return await _db.PlatformContractViews.CountAsync(v => v.PlatformContractId == contractId);
        }
    }
}
