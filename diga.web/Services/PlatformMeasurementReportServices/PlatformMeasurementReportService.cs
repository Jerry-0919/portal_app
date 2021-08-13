using diga.bl.Constants;
using diga.bl.Models.Platform.Contracts;
using diga.bl.Models.Platform.Estimates;
using diga.bl.Models.Platform.MeasurementReport;
using diga.dal.DbServices.PlatformContractChangeDbServices;
using diga.dal.DbServices.PlatformContractDbServices;
using diga.dal.DbServices.PlatformEstimateDbServices;
using diga.dal.DbServices.PlatformEstimatePositionDbServices;
using diga.dal.DbServices.PlatformEstimateSectionDbServices;
using diga.dal.DbServices.PlatformEstimateVATServices;
using diga.dal.DbServices.PlatformMeasurementReportDbServices;
using diga.dal.DbServices.PlatformMeasurementReportPositionDbServices;
using diga.web.Helpers;
using diga.web.Models.PlatformEstimates;
using diga.web.Models.PlatformMeasurementReports;
using diga.web.Models.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformMeasurementReportServices
{
    public class PlatformMeasurementReportService : IPlatformMeasurementReportService
    {
        private readonly IPlatformContractDbService _platformContractDbService;
        private readonly IPlatformEstimateDbService _platformEstimateDbService;
        private readonly IPlatformMeasurementReportDbService _platformMeasurementReportDbService;
        private readonly IPlatformMeasurementReportPositionDbService _platformMeasurementReportPositionDbService;
        private readonly IPlatformContractChangeDbService _platformContractChangeDbService;
        private readonly IPlatformEstimateSectionDbService _platformEstimateSectionDbService;
        private readonly IPlatformEstimatePositionDbService _platformEstimatePositionDbService;
        private readonly IPlatformEstimateVATDbService _platformEstimateVATDbService;

        public PlatformMeasurementReportService(IPlatformContractDbService platformContractDbService,
            IPlatformMeasurementReportDbService platformMeasurementReportDbService,
            IPlatformEstimateDbService platformEstimateDbService,
            IPlatformMeasurementReportPositionDbService platformMeasurementReportPositionDbService,
            IPlatformContractChangeDbService platformContractChangeDbService,
            IPlatformEstimateSectionDbService platformEstimateSectionDbService,
            IPlatformEstimatePositionDbService platformEstimatePositionDbService,
            IPlatformEstimateVATDbService platformEstimateVATDbService)
        {
            _platformContractDbService = platformContractDbService;
            _platformEstimateDbService = platformEstimateDbService;
            _platformMeasurementReportDbService = platformMeasurementReportDbService;
            _platformMeasurementReportPositionDbService = platformMeasurementReportPositionDbService;
            _platformContractChangeDbService = platformContractChangeDbService;
            _platformEstimateSectionDbService = platformEstimateSectionDbService;
            _platformEstimatePositionDbService = platformEstimatePositionDbService;
            _platformEstimateVATDbService = platformEstimateVATDbService;
        }

        public async Task<ResponseData> CancelPublication(int id, int userId)
        {
            var report = await _platformMeasurementReportDbService.Get(id);
            var isExecutor = await _platformContractDbService.IsExecutor(report.PlatformContractId, userId);
            if (!isExecutor)
                return ResponseData.Fail("User", "Only executor can cancel publication");
            if (report.Status != PlatformMeasurementReportStatuses.Published)
                return ResponseData.Fail("Report", "You can only cancel published contract");
            report.Status = PlatformMeasurementReportStatuses.Created;
            await _platformMeasurementReportDbService.Update(report);
            return ResponseData.Ok();
        }

        public async Task<ResponseData> Create(int contractId, int userId)
        {
            var contract = await _platformContractDbService.Get(contractId);

            if ((await _platformContractDbService.IsExecutor(contractId, userId)) == false)
                return ResponseData.Fail("User", "Only executor can create report");

            PlatformEstimate estimate = null;
            var measurementReports = await _platformMeasurementReportDbService.List(contractId);
            if (contract.MainEstimateId > 0)
            {
                estimate = await _platformEstimateDbService.GetWithSections(contract.MainEstimateId.Value);
            }

            var measurementReport = new PlatformMeasurementReport
            {
                Date = DateTime.Now,
                PriceWithVat = 0,
                PriceWithoutVat = 0,
                Version = measurementReports.Count > 0 ? (measurementReports[0].Version++) : 1,
                IsPaid = false,
                PlatformContractId = contractId,
                Status = PlatformMeasurementReportStatuses.Created
            };
            var result = await _platformMeasurementReportDbService.Add(measurementReport);

            if (estimate != null)
            {
                foreach (var section in estimate.Sections)
                {
                    foreach (var position in section.Positions)
                    {
                        await _platformMeasurementReportPositionDbService.Add(new PlatformMeasurementReportPosition
                        {
                            ReportId = result.Id,
                            PositionId = position.Id,
                            PercentCompleted = 0,
                            QuantityCompleted = 0,
                            Status = PlatformMeasurementPositionStatuses.Created
                        });
                    }
                }
            }

            return ResponseData.OkWithData(new { id = result.Id });
        }

        public async Task<ResponseData> Delete(int id, int userId)
        {
            var report = await _platformMeasurementReportDbService.Get(id);
            var isExecutor = await _platformContractDbService.IsExecutor(report.PlatformContractId, userId);
            if (!isExecutor)
                return ResponseData.Fail("User", "Only executor can remove the report");
            if (report.Status != PlatformMeasurementReportStatuses.Created)
                return ResponseData.Fail("Report", "Report cannot be deleted if already published");

            var reportPositions = await _platformMeasurementReportPositionDbService.List(report.Id);
            await _platformMeasurementReportPositionDbService.RemoveRange(reportPositions);
            await _platformMeasurementReportDbService.Remove(report);
            return ResponseData.Ok();
        }

        public async Task<ResponseData> Edit(int id, int userId, PlatformMeasurementReportEditDto request)
        {
            if ((await _platformContractDbService.IsCustomerOrExecutor(request.ContractId, userId)) == false)
                return ResponseData.Fail("User", "Only customer and executor can change the report");
            var isExecutor = await _platformContractDbService.IsExecutor(request.ContractId, userId);
            var isCustomer = await _platformContractDbService.IsCustomer(request.ContractId, userId);

            var report = await _platformMeasurementReportDbService.Get(id);
            var changes = new List<PlatformContractChange>();

            if (report.Status == PlatformMeasurementReportStatuses.Created && isCustomer == true)
                return ResponseData.Fail("User", "Only executor can change report when it is not published");

            var contract = await _platformContractDbService.Get(report.PlatformContractId);

            if (report.Status == PlatformMeasurementReportStatuses.Accepted)
            {
                if (report.InvoiceFile != request.InvoiceFile)
                {
                    changes.Add(new PlatformContractChange(userId, request.ContractId, PlatformContractChangeCases.MeasurementInvoiceUploaded,
                             report.InvoiceFileName, request.InvoiceFileName, DateTime.UtcNow));
                    report.InvoiceFile = request.InvoiceFile;
                    report.InvoiceFileName = request.InvoiceFileName;
                    //todo notify
                }

                if (report.ProofFile != request.ProofFile)
                {
                    changes.Add(new PlatformContractChange(userId, request.ContractId, PlatformContractChangeCases.MeasurementProofUploaded,
                             report.ProofFileName, request.ProofFileName, DateTime.UtcNow));

                    report.ProofFile = request.ProofFile;
                    report.ProofFileName = request.ProofFileName;
                    //todo notify
                }

                if (report.IsPaid != request.IsPaid)
                {
                    report.IsPaid = request.IsPaid;
                    changes.Add(new PlatformContractChange(userId, request.ContractId, PlatformContractChangeCases.MeasurementIsPaid,
                             false.ToString(), true.ToString(), DateTime.UtcNow));
                    //todo notify
                }
                await _platformMeasurementReportDbService.Update(report);
            }

            if (report.Status == PlatformMeasurementReportStatuses.Created || report.Status == PlatformMeasurementReportStatuses.Rejected || report.Status == PlatformMeasurementReportStatuses.Published)
            {
                if (isCustomer == true && report.Status == PlatformMeasurementReportStatuses.Published)
                {
                    changes.Add(new PlatformContractChange(userId, request.ContractId, PlatformContractChangeCases.MeasurementStatusChanged,
                        report.Status, request.Status, DateTime.UtcNow));
                    report.Status = request.Status;
                }

                var priceWithoutVat = 0.0;
                var priceWithVat = 0.0;
                foreach (var p in request.Positions)
                {
                    var measurementPosition = await _platformMeasurementReportPositionDbService.GetFull(p.Id);

                    if (measurementPosition.QuantityCompleted != p.QuantityCompleted && isExecutor == true)
                    {
                        measurementPosition.PercentCompleted = measurementPosition.PlatformEstimatePosition.Quantity > 0 ? p.QuantityCompleted * 100 / measurementPosition.PlatformEstimatePosition.Quantity : 100;
                        measurementPosition.QuantityCompleted = p.QuantityCompleted;
                    }

                    if (measurementPosition.Status != p.Status && isCustomer == true && (p.Status == PlatformMeasurementPositionStatuses.Accepted || p.Status == PlatformMeasurementPositionStatuses.Rejected))
                    {
                        measurementPosition.Status = p.Status;
                        measurementPosition.RejectionReason = p.RejectionReason;
                    }

                    priceWithoutVat += measurementPosition.QuantityCompleted * measurementPosition.PlatformEstimatePosition.Price ?? 0;
                    await _platformMeasurementReportPositionDbService.Update(measurementPosition);
                }
                
                priceWithVat = priceWithoutVat;

                if (contract.MainEstimateId > 0){
                    var estimateVats = await _platformEstimateVATDbService.List(contract.MainEstimateId.Value);
                    foreach(var estimateVat in estimateVats)
                    {
                        priceWithVat += priceWithoutVat * (estimateVat.Percent / 100) * (estimateVat.PlatformVAT.Percent / 100);
                    }
                }

                if (report.PriceWithoutVat != priceWithoutVat){
                    changes.Add(new PlatformContractChange(userId, request.ContractId, PlatformContractChangeCases.MeasurementPriceChanged,
                        report.PriceWithVat.ToString(), priceWithVat.ToString(), DateTime.UtcNow));  
                    report.PriceWithoutVat = priceWithoutVat;
                    report.PriceWithVat = priceWithVat;                  
                }              
            }

            if (changes.Count > 0){
                await _platformContractChangeDbService.AddRange(changes);
                await _platformMeasurementReportDbService.Update(report);
            }

            return ResponseData.Ok();
        }

        public async Task<PlatformMeasurementReportDto> Get(int id)
        {
            var r = await _platformMeasurementReportDbService.Get(id);
            PlatformEstimateFullDto estimateFull = null;
            var contract = await _platformContractDbService.Get(r.PlatformContractId);
            if (contract.MainEstimateId > 0)
            {
                var estimate = await _platformEstimateDbService.GetFull(contract.MainEstimateId.Value);
                estimateFull = PlatformEstimateHelper.Parse(estimate,
                await _platformEstimateSectionDbService.List(estimate.Id),
                await _platformEstimatePositionDbService.List(estimate.Id), true, r.Id);
            }
            return new PlatformMeasurementReportDto
            {
                Id = r.Id,
                Date = r.Date,
                InvoiceFile = r.InvoiceFile,
                InvoiceFileName = r.InvoiceFileName,
                IsPaid = r.IsPaid,
                IsMade = r.IsMade,
                PriceWithoutVat = r.PriceWithoutVat,
                PriceWithVat = r.PriceWithVat,
                ProofFile = r.ProofFile,
                ProofFileName = r.ProofFileName,
                Status = r.Status,
                Version = r.Version,
                Estimate = estimateFull
            };
        }

        public async Task<List<PlatformMeasurementReportDto>> List(int contractId, int userId)
        {
            if ((await _platformContractDbService.IsCustomerOrExecutor(contractId, userId)) == false)
                throw new Exception("Not allowed");

            return (await _platformMeasurementReportDbService.List(contractId)).Select(r => new PlatformMeasurementReportDto
            {
                Id = r.Id,
                Date = r.Date,
                InvoiceFile = r.InvoiceFile,
                InvoiceFileName = r.InvoiceFileName,
                IsPaid = r.IsPaid,
                IsMade = r.IsMade,
                PriceWithoutVat = r.PriceWithoutVat,
                PriceWithVat = r.PriceWithVat,
                ProofFile = r.ProofFile,
                ProofFileName = r.ProofFileName,
                Status = r.Status,
                Version = r.Version
            }).ToList();
        }

        public async Task<ResponseData> Publish(int id, int userId)
        {
            var report = await _platformMeasurementReportDbService.Get(id);
            var isExecutor = await _platformContractDbService.IsExecutor(report.PlatformContractId, userId);
            if (!isExecutor)
                return ResponseData.Fail("User", "Only executor can publish the report");
            report.Status = PlatformMeasurementReportStatuses.Published;
            await _platformMeasurementReportDbService.Update(report);
            return ResponseData.Ok();
        }
    }
}
