using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.ManyToManyDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractTagDbServices
{
    public class PlatformContractTagDbService : ManyToManyDbService<PlatformContractTag>, IPlatformContractTagDbService
    {
        private readonly DigaContext _db;

        public PlatformContractTagDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<List<PlatformContractTag>> List(int contractId)
        {
            return await _db.PlatformContractTags.Where(t => t.PlatformContractId == contractId).ToListAsync();
        }
    }
}
