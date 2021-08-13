using diga.web.Models.PlatformMeasurementReports;
using diga.web.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformMeasurementReportServices
{
    public interface IPlatformMeasurementReportService
    {
        Task<ResponseData> Edit(int id, int userId, PlatformMeasurementReportEditDto request);
        Task<ResponseData> Publish(int id, int userId);
        Task<ResponseData> CancelPublication(int id, int userId);
        Task<PlatformMeasurementReportDto> Get(int id);
        Task<List<PlatformMeasurementReportDto>> List(int contractId, int userId);
        Task<ResponseData> Create(int contractId, int userId);
        Task<ResponseData> Delete(int id, int userId);
    }
}
