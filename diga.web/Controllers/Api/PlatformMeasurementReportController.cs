using diga.dal;
using diga.web.Models.PlatformMeasurementReports;
using diga.web.Models.Status;
using diga.web.Services.PlatformMeasurementReportServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [Route("api/platform/measurementReports")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PlatformMeasurementReportController : BaseController
    {
        private readonly IPlatformMeasurementReportService _platformMeasurementReportService;

        public PlatformMeasurementReportController(IPlatformMeasurementReportService platformMeasurementReportService)
            : base(null, null)
        {
            _platformMeasurementReportService = platformMeasurementReportService;
        }

        [HttpGet("list/{contractId}")]
        public async Task<List<PlatformMeasurementReportDto>> List(int contractId)
        {
            return await _platformMeasurementReportService.List(contractId, UserId);
        }

        [HttpGet("{id}")]
        public async Task<PlatformMeasurementReportDto> Get(int id)
        {
            return await _platformMeasurementReportService.Get(id);
        }

        [HttpPost("{contractId}")]
        public async Task<ResponseData> Post(int contractId)
        {
            return await _platformMeasurementReportService.Create(contractId, UserId);
        }

        [HttpPut("{id}")]
        public async Task<ResponseData> Edit(int id, PlatformMeasurementReportEditDto request)
        {
            return await _platformMeasurementReportService.Edit(id, UserId, request);
        }

        [HttpDelete("{id}")]
        public async Task<ResponseData> Delete(int id)
        {
            return await _platformMeasurementReportService.Delete(id, UserId);
        }

        [HttpPost("{id}/publish")]
        public async Task<ResponseData> Publish(int id)
        {
            return await _platformMeasurementReportService.Publish(id, UserId);
        }

        [HttpPost("{id}/cancelPublication")]
        public async Task<ResponseData> CancelPublication(int id)
        {
            return await _platformMeasurementReportService.CancelPublication(id, UserId);
        }
    }
}
