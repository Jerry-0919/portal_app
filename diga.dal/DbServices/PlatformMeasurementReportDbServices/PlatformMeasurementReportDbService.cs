using diga.bl.Models.Platform.MeasurementReport;
using diga.dal.DbServices.DefaultDbServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformMeasurementReportDbServices
{
    public class PlatformMeasurementReportDbService : DefaultDbService<int, PlatformMeasurementReport>, IPlatformMeasurementReportDbService
    {
        private readonly DigaContext _db;

        public PlatformMeasurementReportDbService(DigaContext db)
            : base(db)
        {
            _db = db;
        }

        public async Task<List<PlatformMeasurementReport>> List(int contractId)
        {
            return await _db.PlatformMeasurementReports.Where(r => r.PlatformContractId == contractId).OrderByDescending(r => r.Date).ToListAsync();
        }
    }
}
