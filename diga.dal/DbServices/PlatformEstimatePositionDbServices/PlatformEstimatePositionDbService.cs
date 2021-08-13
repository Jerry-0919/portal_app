using diga.bl.Models.Platform.Estimates;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformEstimatePositionDbServices
{
    public class PlatformEstimatePositionDbService : DefaultDbService<int, PlatformEstimatePosition>, IPlatformEstimatePositionDbService
    {
        private readonly DigaContext _db;

        public PlatformEstimatePositionDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<double> CalculatePrice(int estimateId)
        {
            var estimate = await _db.PlatformEstimates.Include(e => e.PlatformEstimateVATs).ThenInclude(v => v.PlatformVAT).FirstOrDefaultAsync(e => e.Id == estimateId);
            List<int> sectionIds = await _db.PlatformEstimateSections
                .Where(s => s.EstimateId == estimateId).Select(s => s.Id).ToListAsync();
            var estimatePrice = await _db.PlatformEstimatePositions.Where(p => sectionIds.Contains(p.EstimateSectionId)).SumAsync(p => p.Price * p.Quantity);
            if (estimate.PlatformEstimateVATs.Count > 0)
            {
                var vatValue = 0.0;
                foreach (var vat in estimate.PlatformEstimateVATs)
                {
                    vatValue += estimatePrice * (vat.Percent / 100) * (vat.PlatformVAT.Percent / 100);
                }
                estimatePrice += vatValue;
            }

            return estimatePrice;
        }

        public async Task<List<PlatformEstimatePosition>> List(int estimateId)
        {
            List<int> sectionIds = await _db.PlatformEstimateSections
                .Where(s => s.EstimateId == estimateId).Select(s => s.Id).ToListAsync();

            return await _db.PlatformEstimatePositions.Include(p => p.PlatformMeasurementReportPositions).Where(p => sectionIds.Contains(p.EstimateSectionId)).ToListAsync();
        }

        public async Task<List<PlatformEstimatePosition>> List(List<int> sectionIds)
        {
            return await _db.PlatformEstimatePositions.Include(p => p.PlatformMeasurementReportPositions).Where(p => sectionIds.Contains(p.EstimateSectionId)).ToListAsync();
        }

        public async Task<List<PlatformEstimatePosition>> ListByIds(List<int> positionIdsToRemove)
        {
            return await _db.PlatformEstimatePositions.Where(p => positionIdsToRemove.Contains(p.Id)).ToListAsync();
        }
    }
}
