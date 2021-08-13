using diga.bl.Models.Platform.Estimates;
using diga.dal.DbServices.ManyToManyDbServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformEstimateVATServices
{
    public class PlatformEstimateVATDbService : ManyToManyDbService<PlatformEstimateVAT>, IPlatformEstimateVATDbService
    {
        private readonly DigaContext _db;

        public PlatformEstimateVATDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<List<PlatformEstimateVAT>> List(int estimateId)
        {
            return await _db.PlatformEstimateVATs.Include(v => v.PlatformVAT).Where(v => v.PlatformEstimateId == estimateId).ToListAsync();
        }
    }
}
