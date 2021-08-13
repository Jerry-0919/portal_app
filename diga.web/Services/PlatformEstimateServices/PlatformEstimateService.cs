using diga.bl.Constants;
using diga.bl.Models.Platform.Contracts;
using diga.bl.Models.Platform.Estimates;
using diga.dal.DbServices.PlatformContractApprovalDbServices;
using diga.dal.DbServices.PlatformContractBidDbServices;
using diga.dal.DbServices.PlatformContractChangeDbServices;
using diga.dal.DbServices.PlatformContractDbServices;
using diga.dal.DbServices.PlatformEstimateDbServices;
using diga.dal.DbServices.PlatformEstimatePositionDbServices;
using diga.dal.DbServices.PlatformEstimateSectionDbServices;
using diga.dal.DbServices.PlatformEstimateVATServices;
using diga.web.Helpers;
using diga.web.Models.PlatformEstimates;
using diga.web.Models.PlatformNotifications;
using diga.web.Models.Status;
using diga.web.Services.PlatformContractCloneServices;
using diga.web.Services.PlatformNotificationServices;
using Diga.Parsing.Models.Estimates;
using Diga.Parsing.Parsers;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static diga.bl.Constants.PlatformContractChangeCases;

namespace diga.web.Services.PlatformEstimateServices
{
    public class PlatformEstimateService : IPlatformEstimateService
    {
        private readonly IPlatformContractDbService _platformContractDbService;
        private readonly IPlatformContractBidDbService _platformContractBidDbService;
        private readonly IPlatformEstimateDbService _platformEstimateDbService;
        private readonly IPlatformEstimateSectionDbService _platformEstimateSectionDbService;
        private readonly IPlatformEstimatePositionDbService _platformEstimatePositionDbService;
        private readonly IPlatformContractCloneService _platformContractCloneService;
        private readonly IPlatformContractChangeDbService _platformContractChangeDbService;
        private readonly IPlatformNotificationService _platformNotificationService;
        private readonly IPlatformEstimateVATDbService _platformEstimateVATDbService;
        private readonly IPlatformContractApprovalDbService _platformContractApprovalDbService;
        private readonly IWebHostEnvironment _environment;

        public PlatformEstimateService(IPlatformContractDbService platformContractDbService,
            IPlatformContractBidDbService platformContractBidDbService,
            IPlatformEstimateDbService platformEstimateDbService,
            IPlatformEstimateSectionDbService platformEstimateSectionDbService,
            IPlatformEstimatePositionDbService platformEstimatePositionDbService,
            IPlatformContractCloneService platformContractCloneService,
            IPlatformContractChangeDbService platformContractChangeDbService,
            IPlatformNotificationService platformNotificationService,
            IPlatformEstimateVATDbService platformEstimateVATDbService,
            IPlatformContractApprovalDbService platformContractApprovalDbService,
            IWebHostEnvironment environment)
        {
            _platformContractDbService = platformContractDbService;
            _platformContractBidDbService = platformContractBidDbService;
            _platformEstimateDbService = platformEstimateDbService;
            _platformEstimateSectionDbService = platformEstimateSectionDbService;
            _platformEstimatePositionDbService = platformEstimatePositionDbService;
            _platformContractCloneService = platformContractCloneService;
            _platformContractChangeDbService = platformContractChangeDbService;
            _platformNotificationService = platformNotificationService;
            _platformEstimateVATDbService = platformEstimateVATDbService;
            _platformContractApprovalDbService = platformContractApprovalDbService;
            _environment = environment;
        }

        public async Task<PlatformEstimateFullDto> Get(int userId, int estimateId)
        {
            PlatformEstimate estimate = await _platformEstimateDbService.GetFull(estimateId);
            List<string> notAllowedStatuses = new List<string> { PlatformContractStatuses.Draft,
                PlatformContractStatuses.Deferred };

            if (estimate.PlatformContract.UserId != userId && notAllowedStatuses.Contains(estimate.PlatformContract.Status))
                return null;

            return PlatformEstimateHelper.Parse(estimate,
                await _platformEstimateSectionDbService.List(estimate.Id),
                await _platformEstimatePositionDbService.List(estimate.Id));
        }

        public async Task<PlatformEstimateFullDto> GetMyBidEstimate(int userId, int contractId)
        {
            PlatformEstimate estimate = await _platformEstimateDbService.GetMyBidEstimate(userId, contractId);

            if (estimate == null)
                return null;

            return PlatformEstimateHelper.Parse(estimate,
                await _platformEstimateSectionDbService.List(estimate.Id),
                await _platformEstimatePositionDbService.List(estimate.Id));
        }

        public async Task<PlatformEstimateFullDto> GetApproval(int userId, int contractId)
        {
            PlatformEstimate estimate = await _platformEstimateDbService.GetActual(contractId);

            if (estimate.PlatformContract.UserId != userId)
                return null;

            return PlatformEstimateHelper.Parse(estimate,
                await _platformEstimateSectionDbService.List(estimate.Id),
                await _platformEstimatePositionDbService.List(estimate.Id));
        }

        public async Task<ResponseData<List<PlatformEstimateVersionsDto>>> GetApprovalVersions(int userId, int contractId)
        {
            if (!await _platformContractDbService.IsCustomerOrExecutor(contractId, userId))
                return ResponseData<List<PlatformEstimateVersionsDto>>.Fail("User", "Access denied");

            return new ResponseData<List<PlatformEstimateVersionsDto>>
            {
                Data = (await _platformEstimateDbService.ExecutorsList(contractId)).Select(e => new PlatformEstimateVersionsDto {
                    EstimateId = e.Id,
                    EstimateName = $"№ {contractId}-{e.Id}",
                }).ToList()
            };
        }

        public async Task<List<PlatformEstimateComparingDto>> ListExecutors(int contractId)
        {
            PlatformContract contract = await _platformContractDbService.GetWithEstimates(contractId);
            List<PlatformContractBid> bids = await _platformContractBidDbService.List(contractId);

            return (await _platformEstimateDbService.GetFull(
                contract.PlatformEstimates.Where(e => e.CreatorId != contract.UserId && bids.Any(b => b.ApplicationUserId == e.CreatorId))
                .Select(e => e.Id).ToList()))
                .Select(e =>
                    new PlatformEstimateComparingDto
                    {
                        Id = e.Id,
                        AnotherPercent = e.AnotherPercent,
                        Name = e.Name,
                        //PlatformVATId = e.PlatformVATId,
                        UserAvatar = e.Creator.Avatar,
                        FullName = $"{e.Creator.Name} {e.Creator.Surname}",
                        Positions = e.Sections.SelectMany(s => s.Positions).Select(p => new PlatformEstimatePositionDto
                        {
                            Id = p.Id,
                            Description = p.Description,
                            Measurement = p.Measurement,
                            Notes = p.Notes,
                            Number = p.Number,
                            Price = p.Price,
                            Quantity = p.Quantity,
                            FullNumber = p.GetFullNumber()
                        }).ToList(),
                        DaysNumber = bids.FirstOrDefault(b => b.ApplicationUserId == e.CreatorId)?.DaysNumber,
                        Price = bids.FirstOrDefault(b => b.ApplicationUserId == e.CreatorId)?.Price
                    }
                ).ToList();
        }

        public async Task<ResponseData> Edit(int userId, int estimateId, PlatformEstimateEditDto request)
        {
            PlatformEstimate estimate = await _platformEstimateDbService.GetFull(estimateId);

            if (estimate.PlatformContract.UserId != userId)
                return ResponseData.Fail("User", "Access denied");
            else if (estimate.PlatformContract.Status == PlatformContractStatuses.Archived)
                return ResponseData.Fail("Status", "This version archived!");

            if (estimate.PlatformContract.Status != PlatformContractStatuses.Draft)
            {
                PlatformContract contract = await _platformContractCloneService.Clone(estimateId);
                PlatformEstimate newEstimate = await _platformEstimateDbService.GetActual(contract.Id);

                request.MapNewIds(estimate, newEstimate);
                estimate = newEstimate;
            }

            await Edit(estimate, request, estimateId);

            return new ResponseData();
        }

        public async Task<ResponseData> EditMyBidEstimate(int userId, int contractId, PlatformEstimateExecutorDto request)
        {
            PlatformEstimate estimate = await _platformEstimateDbService.GetMyBidEstimate(userId, contractId);

            List<PlatformEstimateSection> sections = await _platformEstimateSectionDbService.List(estimate.Id);
            List<PlatformEstimatePosition> positions = await _platformEstimatePositionDbService.List(estimate.Id);

            foreach (PlatformEstimateExecutorSectionDto section in request.Sections)
                sections.First(s => s.Id == section.Id).Notes = section.Notes;

            foreach (PlatformEstimateExecutorPositionDto position in request.Positions)
            {
                PlatformEstimatePosition positionToUpdate = positions.First(s => s.Id == position.Id);

                positionToUpdate.Notes = position.Notes;
                positionToUpdate.Price = position.Price ?? 0;
            }

            await _platformEstimateSectionDbService.UpdateRange(sections);
            await _platformEstimatePositionDbService.UpdateRange(positions);

            return new ResponseData();
        }

        public async Task<ResponseData> Approve(int userId, int contractId, int estimateId)
        {
            PlatformContract contract = await _platformContractDbService.GetApproval(contractId);
            int executorId = contract.Bids
                .First(b => b.Status == PlatformContractBidStatuses.Accepted).ApplicationUserId;

            if (contract.Status != PlatformContractStatuses.EstimateApproval)
                return ResponseData.Fail("Status", "Estimate can approve only after selection executor!");

            if (executorId != userId && contract.UserId != userId)
                return ResponseData.Fail("User", "Access denied");
            else if (contract.Status == PlatformContractStatuses.Archived)
                return ResponseData.Fail("Status", "This version archived!");

            if (executorId == userId)
                contract.Approval.EstimateExecutor = true;
            else if (contract.UserId == userId)
                contract.Approval.EstimateCustomer = true;
            contract.MainEstimateId = estimateId;

            await _platformContractChangeDbService.Add(new PlatformContractChange(userId, contractId,
                ApproveEstimate, false.ToString(), true.ToString(), DateTime.UtcNow));

            if (contract.Approval.EstimateExecutor && contract.Approval.EstimateCustomer)
            {
                PlatformEstimate estimate = await _platformEstimateDbService.GetActual(contractId);
                contract.Price = await _platformEstimatePositionDbService.CalculatePrice(estimate.Id);
                contract.Status = PlatformContractStatuses.ContractApproval;
            }

            await _platformContractDbService.Update(contract);

            return new ResponseData();
        }

        private async Task<ResponseData> ApprovalEdit(int userId, PlatformEstimate oldEstimate, PlatformEstimate newEstimate,
            PlatformEstimateEditDto request, PlatformContractBid acceptedBid, List<PlatformContractChange> changes)
        {
            PlatformContractApproval approval = await _platformContractApprovalDbService.Get(newEstimate.PlatformContractId);
            approval.EstimateCustomer = false;
            approval.EstimateExecutor = false;
            await _platformContractApprovalDbService.Update(approval);

            if (oldEstimate != null)
                request.MapNewIds(oldEstimate, newEstimate);

            await Edit(newEstimate, request, newEstimate.Id);
            await _platformContractChangeDbService.AddRange(changes);

            await _platformNotificationService.AddNotification(
                newEstimate.PlatformContract.UserId == userId ? acceptedBid.ApplicationUserId : newEstimate.PlatformContract.UserId,
                new PlatformNotificationDto
                {
                    IsRead = false,
                    DateTime = DateTime.UtcNow,
                    Type = PlatformNotificationTypes.EstimateChanged,
                    Datas = new List<PlatformNotificationDataDto> {
                        new PlatformNotificationDataDto("ContractId", newEstimate.PlatformContractId.ToString())
                    }
                });

            return new ResponseData();
        }

        public async Task<ResponseData> ApprovalEdit(int userId, int estimateId, PlatformEstimateEditDto request)
        {
            PlatformEstimate estimate = await _platformEstimateDbService.GetApproval(estimateId);
            PlatformEstimate actualEstimate = await _platformEstimateDbService.GetActualByEstimateId(estimate.Id);

            PlatformContractBid acceptedBid = await _platformContractBidDbService.GetAccepted(estimate.PlatformContractId);

            if (estimate.PlatformContract.UserId != userId && acceptedBid.ApplicationUserId != acceptedBid.ApplicationUserId)
                return ResponseData.Fail("User", "Access denied!");
            else if (estimate.PlatformContract.Status == PlatformContractStatuses.Archived)
                return ResponseData.Fail("Status", "This version archived!");
            // else if (actualEstimate.Id != estimate.Id)
            //     return ResponseData.Fail("VersionNumber", "Version is not actual");

            List<PlatformContractChange> changes = GetChanges(estimate, userId, request);

            return await ApprovalEdit(userId, null, estimate, request, acceptedBid, changes);
        }

        public async Task<ResponseData> ApprovalCloneEdit(int userId, int estimateId, PlatformEstimateEditDto request)
        {
            PlatformEstimate estimate = await _platformEstimateDbService.GetApproval(estimateId);
            PlatformEstimate actualEstimate = await _platformEstimateDbService.GetActualByEstimateId(estimate.Id);

            PlatformContractBid acceptedBid = await _platformContractBidDbService.GetAccepted(estimate.PlatformContractId);

            if (estimate.PlatformContract.UserId != userId && acceptedBid.ApplicationUserId != acceptedBid.ApplicationUserId)
                return ResponseData.Fail("User", "Access denied!");
            else if (estimate.PlatformContract.Status == PlatformContractStatuses.Archived)
                return ResponseData.Fail("Status", "This version archived!");
            else if (actualEstimate.Id != estimate.Id)
                return ResponseData.Fail("VersionNumber", "Version is not actual");

            List<PlatformContractChange> changes = GetChanges(estimate, userId, request);

            PlatformEstimate newEstimate = await _platformContractCloneService.CloneEstimate(
                estimate.Id, estimate.PlatformContract.UserId, actualEstimate.VersionNumber + 1);
            newEstimate = await _platformEstimateDbService.GetWithSections(newEstimate.Id);

            return await ApprovalEdit(userId, estimate, newEstimate, request, acceptedBid, changes);
        }

        public async Task<ResponseData> ApprovalMain(int userId, int estimateId)
        {
            PlatformEstimate estimate = await _platformEstimateDbService.GetApproval(estimateId);
            PlatformContractBid acceptedBid = await _platformContractBidDbService.GetAccepted(estimate.PlatformContractId);

            if (estimate.PlatformContract.UserId != userId && acceptedBid.ApplicationUserId != acceptedBid.ApplicationUserId)
                return ResponseData.Fail("User", "Access denied!");
            else if (estimate.PlatformContract.Status == PlatformContractStatuses.Archived)
                return ResponseData.Fail("Status", "This version archived!");

            List<int> estimateIds = await _platformEstimateDbService.GetExecutorVersionIds(estimate.PlatformContractId);
            List<PlatformEstimate> estimates = await _platformEstimateDbService.List(estimateIds);
            List<PlatformEstimate> estimatesToUpdate = estimates.Where(e => e.VersionNumber > estimate.VersionNumber).ToList();

            foreach (PlatformEstimate estimateToUpdate in estimatesToUpdate)
                estimateToUpdate.VersionNumber--;

            estimate.VersionNumber = estimatesToUpdate.Max(e => e.VersionNumber) + 1;
            estimatesToUpdate.Add(estimate);

            await _platformEstimateDbService.UpdateRange(estimatesToUpdate);

            return ResponseData.Ok();
        }

        private List<PlatformContractChange> GetChanges(PlatformEstimate estimate, int userId, PlatformEstimateEditDto request)
        {
            DateTime now = DateTime.UtcNow;
            List<PlatformEstimatePosition> positions = estimate.Sections.SelectMany(s => s.Positions).ToList();

            List<PlatformContractChange> changes = new List<PlatformContractChange> { };
            changes.CompareWrite(estimate.AnotherPercent.ToString(), request.AnotherPercent.ToString(), userId, estimate.PlatformContractId, AnotherPercent, now);
            // changes.CompareWrite(estimate.Name, request.Name, userId, estimate.PlatformContractId, EstimateName, now);
            //changes.CompareWrite(estimate.PlatformVATId.ToString(), request.PlatformVATId.ToString(), userId, estimate.PlatformContractId, PlatformVATId, now);

            changes.AddRange(request.SectionsToAdd.Select(s => new PlatformContractChange(userId, estimate.PlatformContractId, AddSection, string.Empty,
                JsonConvert.SerializeObject(new { s.Name, s.Number, s.ParentSectionId, s.Notes }), now)));

            changes.AddRange(request.SectionsToUpdate.Select(s =>
            {
                PlatformEstimateSection section = estimate.Sections.First(es => es.Id == s.Id);
                return new PlatformContractChange(userId, estimate.PlatformContractId, EditSection,
                    JsonConvert.SerializeObject(new { section.Id, section.Name, section.Number, s.ParentSectionId, s.Notes }),
                    JsonConvert.SerializeObject(new { s.Id, s.Name, s.Number, s.ParentSectionId, s.Notes }), now);
            }));

            changes.AddRange(request.SectionIdsToRemove.Select(s =>
            {
                PlatformEstimateSection section = estimate.Sections.First(es => es.Id == s);
                return new PlatformContractChange(userId, estimate.PlatformContractId, RemoveSection,
                      JsonConvert.SerializeObject(new { section.Id, section.Name, section.Number, section.ParentSectionId, section.Notes }), string.Empty, now);
            }));

            changes.AddRange(request.PositionsToAdd.Select(p => new PlatformContractChange(userId, estimate.PlatformContractId, AddPosition, string.Empty,
                JsonConvert.SerializeObject(new { p.Number, p.Description, p.Measurement, p.Notes, p.Price, p.Quantity, p.SectionId }), now)));

            changes.AddRange(request.PositionsToUpdate.Select(p =>
            {
                PlatformEstimatePosition position = positions.First(ep => ep.Id == p.Id);
                return new PlatformContractChange(userId, estimate.PlatformContractId, EditPosition,
                    JsonConvert.SerializeObject(new { position.Id, position.Number, position.Description, position.Measurement, position.Notes, position.Price, position.Quantity, position.EstimateSectionId }),
                    JsonConvert.SerializeObject(new { p.Id, p.Number, p.Description, p.Measurement, p.Notes, p.Price, p.Quantity, p.SectionId }), now);
            }));

            changes.AddRange(request.PositionIdsToRemove.Select(p =>
            {
                PlatformEstimatePosition position = positions.First(ep => ep.Id == p);
                return new PlatformContractChange(userId, estimate.PlatformContractId, RemovePosition,
                      JsonConvert.SerializeObject(new { position.Id, position.Number, position.Description, position.Measurement, position.Notes, position.Price, position.Quantity, position.EstimateSectionId }), string.Empty, now);
            }));

            return changes;
        }

        #region Main

        public async Task<int> ParseAdd(string name, string fileName, bool isOrdered, int contractId, int creatorId, int versionNumber)
        {
            PlatformEstimate platformEstimate = await _platformEstimateDbService.Add(new PlatformEstimate
            {
                Name = name,
                FileName = fileName,
                IsOrdered = isOrdered,
                PlatformContractId = contractId,
                CreatorId = creatorId,
                VersionNumber = versionNumber,
                VatType = PlatformEstimateVatTypes.Private
            });

            if (string.IsNullOrEmpty(fileName))
                return platformEstimate.Id;

            string tempPath = Path.Combine(_environment.WebRootPath, "docs", "temp", fileName);
            string srcPath = Path.Combine(_environment.WebRootPath, "docs", "src", fileName);

            File.Move(tempPath, srcPath);

            string extension = Path.GetExtension(fileName);
            if (extension == ".xls" || extension == ".xlsx")
            {
                EstimateParser parser = new EstimateParser(new FileStream(srcPath, FileMode.Open, FileAccess.Read));
                EstimateDto estimate = parser.ParseEstimate();

                if (estimate.FileName == "Failed")
                    throw new Exception("Parse failed!");

                List<PlatformEstimateSection> sections = estimate.Sections.Select(s => new PlatformEstimateSection
                {
                    EstimateId = platformEstimate.Id,
                    Name = s.Name,
                    Number = int.Parse(s.Number.Split('.').Last()),
                    FullNumber = s.Number
                }).ToList();

                await _platformEstimateSectionDbService.AddRange(sections);

                foreach (PlatformEstimateSection section in sections)
                {
                    EstimateSectionDto sectionDto = estimate.Sections.First(s => s.Number == section.FullNumber);
                    section.ParentSectionId = sections.FirstOrDefault(s => s.FullNumber == sectionDto.ParentSection?.Number)?.Id;
                }

                await _platformEstimateSectionDbService.UpdateRange(sections);

                List<PlatformEstimatePosition> positions = estimate.Positions.Select(p => new PlatformEstimatePosition
                {
                    Description = p.Description,
                    Measurement = p.Measurement,
                    Quantity = p.Quantity,
                    Number = int.Parse(p.Number.Split('.').Last()),
                    FullNumber = p.Number
                }).ToList();

                foreach (PlatformEstimatePosition position in positions)
                {
                    EstimatePositionDto positionDto = estimate.Positions.First(s => s.Number == position.FullNumber);
                    position.EstimateSectionId = sections.First(s => s.FullNumber == positionDto.Section.Number).Id;
                }

                await _platformEstimatePositionDbService.AddRange(positions);
            }
            return platformEstimate.Id;
        }

        private async Task Edit(PlatformEstimate estimate, PlatformEstimateEditDto request, int estimateId)
        {
            // estimate.Name = request.Name;
            //estimate.PlatformVATId = request.PlatformVATId;
            if (request.VatType != PlatformEstimateVatTypes.NotCharged && request.VatType != PlatformEstimateVatTypes.Simple && request.VatType != PlatformEstimateVatTypes.Private)
            {
                throw new Exception("estimate vatType is not valid");
            }

            estimate.VatType = request.VatType;
            estimate.AnotherPercent = request.AnotherPercent;

            if (request.PlatformEstimateVats.Count > 0)
            {
                var newVatIds = request.PlatformEstimateVats.Select(v => v.PlatformVATId).Distinct().ToList();
                var existingVats = await _platformEstimateVATDbService.List(estimateId);
                var existingVatIds = existingVats.Select(v => v.PlatformVATId).Distinct().ToList();
                await _platformEstimateVATDbService.AddRange(request.PlatformEstimateVats.Where(v => !existingVatIds.Contains(v.PlatformVATId)).Select(v => new PlatformEstimateVAT
                {
                    PlatformEstimateId = v.PlatformEstimateId,
                    PlatformVATId = v.PlatformVATId,
                    Percent = v.Percent
                }).ToList());
                await _platformEstimateVATDbService.RemoveRange(existingVats.Where(v => !newVatIds.Contains(v.PlatformVATId)).ToList());
                
                foreach(var toUpdate in existingVats.Where(v => newVatIds.Contains(v.PlatformVATId)).ToList()){
                    toUpdate.Percent = request.PlatformEstimateVats.FirstOrDefault(v => v.PlatformVATId == toUpdate.PlatformVATId).Percent;
                    await _platformEstimateVATDbService.Update(toUpdate);
                }
            }
            else{
                var existingVats = await _platformEstimateVATDbService.List(estimateId);
                if (existingVats.Count > 0){
                    await _platformEstimateVATDbService.RemoveRange(existingVats);
                }
            }

            if (request.SectionsToAdd.Count != 0)
            {
                await _platformEstimateSectionDbService.AddRange(request.SectionsToAdd.Select(s => new PlatformEstimateSection
                {
                    EstimateId = estimateId,
                    Name = s.Name,
                    Number = s.Number,
                    ParentSectionId = s.ParentSectionId
                }));
            }

            if (request.SectionsToUpdate.Count != 0)
            {
                List<PlatformEstimateSection> sectionsForUpdate =
                    await _platformEstimateSectionDbService.ListByIds(request.SectionsToUpdate.Select(s => s.Id).ToList());

                await _platformEstimateSectionDbService.UpdateRange(sectionsForUpdate.Select(s =>
                {
                    PlatformEstimateSectionEditDto editSection = request.SectionsToUpdate.First(su => su.Id == s.Id);
                    s.Name = editSection.Name;
                    s.Number = editSection.Number;
                    s.ParentSectionId = editSection.ParentSectionId;
                    return s;
                }));
            }

            if (request.SectionIdsToRemove.Count != 0)
            {
                List<PlatformEstimateSection> sections = await _platformEstimateSectionDbService.ListByIds(request.SectionIdsToRemove);
                await _platformEstimateSectionDbService.RemoveRange(sections);
            }

            if (request.PositionsToAdd.Count != 0)
            {
                await _platformEstimatePositionDbService.AddRange(request.PositionsToAdd.Select(p => new PlatformEstimatePosition
                {
                    Measurement = p.Measurement,
                    Description = p.Description,
                    EstimateSectionId = p.SectionId,
                    Number = p.Number,
                    Quantity = p.Quantity
                }));
            }

            if (request.PositionsToUpdate.Count != 0)
            {
                List<PlatformEstimatePosition> positionsForUpdate =
                    await _platformEstimatePositionDbService.ListByIds(request.PositionsToUpdate.Select(s => s.Id).ToList());

                await _platformEstimatePositionDbService.UpdateRange(positionsForUpdate.Select(s =>
                {
                    PlatformEstimatePositionEditDto editPosition = request.PositionsToUpdate.First(su => su.Id == s.Id);
                    s.Description = editPosition.Description;
                    s.Measurement = editPosition.Measurement;
                    s.Notes = editPosition.Notes;
                    s.Price = editPosition.Price;
                    s.Quantity = editPosition.Quantity;
                    s.Number = editPosition.Number;
                    return s;
                }));

                //await _platformEstimatePositionDbService.UpdateRange(request.PositionsToUpdate.Select(p => new PlatformEstimatePosition
                //{
                //    Id = p.Id,
                //    Measurement = p.Measurement,
                //    Description = p.Description,
                //    EstimateSectionId = p.SectionId,
                //    Number = p.Number,
                //    Quantity = p.Quantity
                //}));
            }

            if (request.PositionIdsToRemove.Count != 0)
            {
                List<PlatformEstimatePosition> positions = await _platformEstimatePositionDbService.ListByIds(request.PositionIdsToRemove);
                await _platformEstimatePositionDbService.RemoveRange(positions);
            }
        }

        public async Task Remove(PlatformEstimate estimate)
        {
            await _platformEstimateDbService.Remove(estimate);

            string oldSrcPath = Path.Combine(_environment.WebRootPath, "docs", "src", estimate.FileName);
            if (File.Exists(oldSrcPath))
                File.Delete(oldSrcPath);
        }

        #endregion
    }
}
