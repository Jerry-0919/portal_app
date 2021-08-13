using diga.bl.Models;
using diga.bl.Models.Platform.Estimates;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformEstimateSectionDbServices
{
    public class PlatformEstimateSectionDbService : DefaultDbService<int, PlatformEstimateSection>, IPlatformEstimateSectionDbService
    {
        private readonly DigaContext _db;

        public PlatformEstimateSectionDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<List<PlatformEstimateSection>> List(int estimateId)
        {
            return await _db.PlatformEstimateSections.Where(p => p.EstimateId == estimateId).ToListAsync();
        }

        public async Task<List<PlatformEstimateSection>> ListByIds(List<int> sectionIdsToRemove)
        {
            return await _db.PlatformEstimateSections.Where(s => sectionIdsToRemove.Contains(s.Id)).ToListAsync();
        }
    }
}
