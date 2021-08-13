using diga.bl.Models.Platform.MeasurementReport;
using diga.dal.DbServices.DefaultDbServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace diga.dal.DbServices.PlatformMeasurementReportPositionDbServices
{
    public interface IPlatformMeasurementReportPositionDbService: IDefaultDbService<int, PlatformMeasurementReportPosition>
    {
        Task<PlatformMeasurementReportPosition> GetFull(int id);
        Task<List<PlatformMeasurementReportPosition>> List(int reportId);
    }
}
