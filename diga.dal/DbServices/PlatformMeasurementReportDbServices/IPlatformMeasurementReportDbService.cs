using diga.bl.Models.Platform.MeasurementReport;
using diga.dal.DbServices.DefaultDbServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformMeasurementReportDbServices
{
    public interface IPlatformMeasurementReportDbService : IDefaultDbService<int, PlatformMeasurementReport>
    {
        Task<List<PlatformMeasurementReport>> List(int contractId);
    }
}
