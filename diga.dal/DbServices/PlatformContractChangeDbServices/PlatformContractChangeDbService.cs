using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractChangeDbServices
{
    public class PlatformContractChangeDbService : DefaultDbService<int, PlatformContractChange>, IPlatformContractChangeDbService
    {
        private DigaContext _db;

        public PlatformContractChangeDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<PlatformContractChange> GetLast(int contractId)
        {
            return await _db.PlatformContractChanges.OrderBy(c => c.DateTime)
                .LastOrDefaultAsync(c => c.PlatformContractId == contractId);
        }

        public async Task<List<PlatformContractChange>> List(int contractId)
        {
            return await _db.PlatformContractChanges.Include(c => c.ApplicationUser).Where(c => c.PlatformContractId == contractId).ToListAsync();
        }
    }
}
