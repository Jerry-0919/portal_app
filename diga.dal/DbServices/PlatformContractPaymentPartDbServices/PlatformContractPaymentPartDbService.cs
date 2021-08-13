using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformContractPaymentPartDbServices
{
    public class PlatformContractPaymentPartDbService : DefaultDbService<int, PlatformContractPaymentPart>, IPlatformContractPaymentPartDbService
    {
        private readonly DigaContext _db;

        public PlatformContractPaymentPartDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<List<PlatformContractPaymentPart>> List(int contractId)
        {
            return await _db.PlatformContractPaymentParts.Where(p => p.PlatformContractId == contractId).ToListAsync();
        }
    }
}
