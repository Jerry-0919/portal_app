using diga.bl.Models.Platform.MeasurementReport;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformMeasurementReportPositionDbServices
{
    class PlatformMeasurementReportPositionDbService : DefaultDbService<int, PlatformMeasurementReportPosition>, IPlatformMeasurementReportPositionDbService
    {
        private readonly DigaContext _db;

        public PlatformMeasurementReportPositionDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<PlatformMeasurementReportPosition> GetFull(int id)
        {
            return await _db.PlatformMeasurementReportPositions.Include(p => p.PlatformEstimatePosition).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<PlatformMeasurementReportPosition>> List(int reportId)
        {
            return await _db.PlatformMeasurementReportPositions.Include(p => p.PlatformEstimatePosition).Where(p => p.ReportId == reportId).ToListAsync();
        }
    }
}
