using diga.bl.Constants;
using diga.bl.Models.Platform.Contracts;
using diga.bl.Models.Platform.Estimates;
using diga.dal.DbServices.PlatformContractDbServices;
using diga.dal.DbServices.PlatformContractFileDbServices;
using diga.dal.DbServices.PlatformContractTagDbServices;
using diga.dal.DbServices.PlatformEstimateDbServices;
using diga.dal.DbServices.PlatformEstimatePositionDbServices;
using diga.dal.DbServices.PlatformEstimateSectionDbServices;
using diga.web.Helpers;
using diga.web.Models.PlatformEstimates;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractCloneServices
{
    public class PlatformContractCloneService : IPlatformContractCloneService
    {
        private readonly IPlatformContractDbService _platformContractDbService;
        private readonly IPlatformContractCategoryDbService _platformContractCategoryDbService;
        private readonly IPlatformContractTagDbService _platformContractTagDbService;
        private readonly IPlatformContractFileDbService _platformContractFileDbService;
        private readonly IPlatformEstimateDbService _platformEstimateDbService;
        private readonly IPlatformEstimateSectionDbService _platformEstimateSectionDbService;
        private readonly IPlatformEstimatePositionDbService _platformEstimatePositionDbService;

        private readonly IWebHostEnvironment _environment;

        public PlatformContractCloneService(IPlatformContractDbService platformContractDbService,
            IPlatformContractCategoryDbService platformContractCategoryDbService,
            IPlatformContractTagDbService platformContractTagDbService,
            IPlatformContractFileDbService platformContractFileDbService,
            IPlatformEstimateDbService platformEstimateDbService,
            IPlatformEstimateSectionDbService platformEstimateSectionDbService,
            IPlatformEstimatePositionDbService platformEstimatePositionDbService,
            IWebHostEnvironment environment)
        {
            _platformContractDbService = platformContractDbService;
            _platformContractCategoryDbService = platformContractCategoryDbService;
            _platformContractTagDbService = platformContractTagDbService;
            _platformContractFileDbService = platformContractFileDbService;
            _platformEstimateDbService = platformEstimateDbService;
            _platformEstimateSectionDbService = platformEstimateSectionDbService;
            _platformEstimatePositionDbService = platformEstimatePositionDbService;

            _environment = environment;
        }

        public async Task<PlatformContract> Clone(int contractId)
        {
            PlatformContract contract = await _platformContractDbService.GetFull(contractId);
            await _platformContractDbService.UpdateNotActual(contractId);

            PlatformContract cloneContract = contract.Clone();
            List<PlatformEstimate> estimates = cloneContract.PlatformEstimates.ToList();

            // Add new contract
            cloneContract.Id = 0;
            cloneContract.Approval = new PlatformContractApproval { };
            cloneContract.OriginalId = contract.OriginalId == null ? contractId : contract.OriginalId;
            cloneContract.VersionNumber = await _platformContractDbService.GetLastVersion(cloneContract.OriginalId.Value) + 1;
            cloneContract.VersionStatus = PlatformContractVersionStatuses.Actual;
            cloneContract.Status = PlatformContractStatuses.Draft;
            cloneContract.EditStatus = PlatformContractEditStatuses.Info;
            cloneContract.CreatedDate = DateTime.UtcNow;
            cloneContract.UpdatedDate = DateTime.UtcNow;
            cloneContract.Tags = new List<PlatformContractTag> { };
            cloneContract.Categories = new List<PlatformContractCategory> { };
            cloneContract.Files = new List<PlatformContractFile> { };

            PlatformContract newContract = await _platformContractDbService.Add(cloneContract);

            await _platformContractCategoryDbService.AddRange(
                contract.Categories.Select(c => new PlatformContractCategory
                {
                    PlatformCategoryId = c.PlatformCategoryId,
                    PlatformContractId = newContract.Id
                }));

            await _platformContractTagDbService.AddRange(
                contract.Tags.Select(c => new PlatformContractTag
                {
                    PlatformTagId = c.PlatformTagId,
                    PlatformContractId = newContract.Id
                }));

            // Map files
            List<string> newFiles = new List<string> { };
            foreach (PlatformContractFile file in contract.Files)
            {
                string fileName = Path.Combine(_environment.WebRootPath, "docs", "src", file.FileName);

                if (File.Exists(fileName))
                {
                    string newPath = Path.Combine(_environment.WebRootPath, "docs", "src",
                         $"{Guid.NewGuid()}{Path.GetExtension(fileName)}");
                    File.Copy(fileName, newPath);

                    newFiles.Add(newPath);
                }
            }

            await _platformContractFileDbService.AddRange(
                newFiles.Select(f => new PlatformContractFile
                {
                    FileName = f,
                    PlatformContractId = newContract.Id
                }));

            // Map estimate
            if (estimates.Any(e => e.CreatorId == contract.UserId))
                await CloneEstimate(estimates.First(e => e.CreatorId == contract.UserId).Id, contract.UserId);

            return newContract;
        }

        public async Task<PlatformEstimate> CloneEstimate(int estimateId, int creatorId, int versionNumber = 1)
        {
            PlatformEstimate estimateForClone = await _platformEstimateDbService.Get(estimateId);
            PlatformEstimate platformEstimate = estimateForClone.Clone();
            platformEstimate.Id = 0;
            platformEstimate.VersionNumber = versionNumber;
            platformEstimate.CreatorId = creatorId;
            platformEstimate.OriginalId = platformEstimate.OriginalId.HasValue ? platformEstimate.OriginalId.Value : platformEstimate.Id;
            if (platformEstimate.PlatformEstimateVATs.Count > 0)
            {
                platformEstimate.PlatformEstimateVATs = estimateForClone.PlatformEstimateVATs.Select(v => new PlatformEstimateVAT { Percent = v.Percent, PlatformVATId = v.PlatformVATId, PlatformVAT = v.PlatformVAT }).ToList();
            }
            platformEstimate = await _platformEstimateDbService.Add(platformEstimate);

            List<PlatformEstimateSection> sections = await _platformEstimateSectionDbService.List(estimateId);
            List<PlatformEstimatePosition> positions = await _platformEstimatePositionDbService.List(estimateId);

            sections = sections.Select(s => s.Clone()).ToList();
            positions = positions.Select(p => p.Clone()).ToList();

            PlatformEstimateFullDto platformEstimateFull = PlatformEstimateHelper.Parse(platformEstimate, sections, positions);
            Tuple<List<PlatformEstimateSection>, List<PlatformEstimatePosition>> tuple =
                PlatformEstimateHelper.Build(platformEstimateFull.Sections, platformEstimate.Id);

            foreach (PlatformEstimateSection section in tuple.Item1)
            {
                if (section.FullNumber.Contains('.'))
                    section.ParentSection = tuple.Item1.First(i => section.FullNumber == $"{i.FullNumber}.{section.Number}");

                section.Positions = tuple.Item2.Where(p => p.FullNumber == $"{section.FullNumber}.{p.Number}").ToList();
                platformEstimate.Sections.Add(section);
            }

            await _platformEstimateSectionDbService.AddRange(tuple.Item1);

            return platformEstimate;
        }
    }
}
