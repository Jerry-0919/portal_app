using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractFileDbServices
{
    public class PlatformContractFileDbService : DefaultDbService<int, PlatformContractFile>, IPlatformContractFileDbService
    {
        private readonly DigaContext _db;

        public PlatformContractFileDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<List<PlatformContractFile>> List(int contractId)
        {
            return await _db.PlatformContractFiles.Where(f => f.PlatformContractId == contractId).ToListAsync();
        }
    }
}
