using diga.bl.Constants;
using diga.bl.Enums.PlatformContracts;
using diga.bl.Models.Platform.Contracts;
using diga.bl.Models.Platform.Estimates;
using diga.bl.Models.Platform.Information;
using diga.dal.DbServices.PlatformBalanceDbServices;
using diga.dal.DbServices.PlatformCategoryDbServices;
using diga.dal.DbServices.PlatformChatRoomMessageDbServices;
using diga.dal.DbServices.PlatformCityDbServices;
using diga.dal.DbServices.PlatformContractApprovalDbServices;
using diga.dal.DbServices.PlatformContractBidDbServices;
using diga.dal.DbServices.PlatformContractChangeDbServices;
using diga.dal.DbServices.PlatformContractDbServices;
using diga.dal.DbServices.PlatformContractFavoriteDbServices;
using diga.dal.DbServices.PlatformContractFileDbServices;
using diga.dal.DbServices.PlatformContractPaymentPartDbServices;
using diga.dal.DbServices.PlatformContractPriorityDbServices;
using diga.dal.DbServices.PlatformContractReviewDbServices;
using diga.dal.DbServices.PlatformContractTagDbServices;
using diga.dal.DbServices.PlatformContractTypeDbServices;
using diga.dal.DbServices.PlatformContractViewDbServices;
using diga.dal.DbServices.PlatformEstimateDbServices;
using diga.dal.DbServices.PlatformEstimatePositionDbServices;
using diga.dal.DbServices.PlatformEstimateSectionDbServices;
using diga.dal.DbServices.PlatformMeasurementReportDbServices;
using diga.dal.DbServices.PlatformMeasurementReportPositionDbServices;
using diga.dal.DbServices.PlatformTagDbServices;
using diga.dal.DbServices.PlatformUserRatingDbServices;
using diga.web.Extensions;
using diga.web.Models.Pagination;
using diga.web.Models.PlatformContracts;
using diga.web.Models.Status;
using diga.web.Services.PlatformContractCloneServices;
using diga.web.Services.PlatformEstimateServices;
using diga.web.Validators.PlatformContracts;
using FluentValidation.Results;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformContractServices
{
    public class PlatformContractService : IPlatformContractService
    {
        private readonly IPlatformContractDbService _platformContractDbService;
        private readonly IPlatformContractBidDbService _platformContractBidDbService;
        private readonly IPlatformContractFileDbService _platformContractFileDbService;
        private readonly IPlatformCategoryDbService _platformCategoryDbService;
        private readonly IPlatformContractCategoryDbService _platformContractCategoryDbService;
        private readonly IPlatformBalanceDbService _platformBalanceDbService;
        private readonly IPlatformContractTypeDbService _platformContractTypeDbService;
        private readonly IPlatformContractPriorityDbService _platformContractPriorityDbService;
        private readonly IPlatformContractReviewDbService _platformContractReviewDbService;
        private readonly IPlatformEstimateDbService _platformEstimateDbService;
        private readonly IPlatformEstimateSectionDbService _platformEstimateSectionDbService;
        private readonly IPlatformEstimatePositionDbService _platformEstimatePositionDbService;
        private readonly IPlatformCityDbService _platformCityDbService;
        private readonly IPlatformContractViewDbService _platformContractViewDbService;
        private readonly IPlatformTagDbService _platformTagDbService;
        private readonly IPlatformContractTagDbService _platformContractTagDbService;
        private readonly IPlatformUserRatingDbService _platformUserRatingDbService;
        private readonly IPlatformContractFavoriteDbService _platformContractFavoriteDbService;
        private readonly IPlatformContractChangeDbService _platformContractChangeDbService;
        private readonly IPlatformContractApprovalDbService _platformContractApprovalDbService;
        private readonly IPlatformChatRoomMessageDbService _platformChatRoomMessageDbService;

        private readonly IPlatformEstimateService _platformEstimateService;
        private readonly IPlatformContractCloneService _platformContractCloneService;
        private readonly IPlatformMeasurementReportDbService _platformMeasurementReportDbService;
        private readonly IPlatformMeasurementReportPositionDbService _platformMeasurementReportPositionDbService;
        private readonly IPlatformContractPaymentPartDbService _platformContractPaymentPartDbService;
        private readonly IWebHostEnvironment _environment;

        private readonly PlatformContractInfoValidator _infoValidator;
        private readonly PlatformContractDatesValidator _datesValidator;
        private readonly PlatformContractCompleteValidator _completeValidator;

        public PlatformContractService(IPlatformContractDbService platformContractDbService,
            IPlatformContractBidDbService platformContractBidDbService,
            IPlatformContractFileDbService platformContractFileDbService,
            IPlatformCategoryDbService platformCategoryDbService,
            IPlatformContractCategoryDbService platformContractCategoryDbService,
            IPlatformBalanceDbService platformBalanceDbService,
            IPlatformContractTypeDbService platformContractTypeDbService,
            IPlatformContractPriorityDbService platformContractPriorityDbService,
            IPlatformContractReviewDbService platformContractReviewDbService,
            IPlatformEstimateDbService platformEstimateDbService,
            IPlatformEstimateSectionDbService platformEstimateSectionDbService,
            IPlatformEstimatePositionDbService platformEstimatePositionDbService,
            IPlatformCityDbService platformCityDbService,
            IPlatformContractViewDbService platformContractViewDbService,
            IPlatformTagDbService platformTagDbService,
            IPlatformContractTagDbService platformContractTagDbService,
            IPlatformEstimateService platformEstimateService,
            IPlatformContractCloneService platformContractCloneService,
            IPlatformUserRatingDbService platformUserRatingDbService,
            IPlatformContractFavoriteDbService platformContractFavoriteDbService,
            IPlatformContractChangeDbService platformContractChangeDbService,
            IPlatformContractApprovalDbService platformContractApprovalDbService,
            IPlatformChatRoomMessageDbService platformChatRoomMessageDbService,
            IPlatformMeasurementReportDbService platformMeasurementReportDbService,
            IPlatformMeasurementReportPositionDbService platformMeasurementReportPositionDbService,
            IPlatformContractPaymentPartDbService platformContractPaymentPartDbService,
            IWebHostEnvironment environment,
            PlatformContractInfoValidator infoValidator,
            PlatformContractDatesValidator datesValidator,
            PlatformContractCompleteValidator completeValidator)
        {
            _platformContractDbService = platformContractDbService;
            _platformContractBidDbService = platformContractBidDbService;
            _platformContractFileDbService = platformContractFileDbService;
            _platformCategoryDbService = platformCategoryDbService;
            _platformContractCategoryDbService = platformContractCategoryDbService;
            _platformBalanceDbService = platformBalanceDbService;
            _platformContractTypeDbService = platformContractTypeDbService;
            _platformContractPriorityDbService = platformContractPriorityDbService;
            _platformContractReviewDbService = platformContractReviewDbService;
            _platformEstimateDbService = platformEstimateDbService;
            _platformEstimateSectionDbService = platformEstimateSectionDbService;
            _platformEstimatePositionDbService = platformEstimatePositionDbService;
            _platformCityDbService = platformCityDbService;
            _platformContractViewDbService = platformContractViewDbService;
            _platformTagDbService = platformTagDbService;
            _platformContractTagDbService = platformContractTagDbService;
            _platformContractCloneService = platformContractCloneService;
            _platformUserRatingDbService = platformUserRatingDbService;
            _platformEstimateService = platformEstimateService;
            _platformContractFavoriteDbService = platformContractFavoriteDbService;
            _platformContractChangeDbService = platformContractChangeDbService;
            _platformContractApprovalDbService = platformContractApprovalDbService;
            _platformChatRoomMessageDbService = platformChatRoomMessageDbService;
            _platformMeasurementReportDbService = platformMeasurementReportDbService;
            _platformMeasurementReportPositionDbService = platformMeasurementReportPositionDbService;
            _platformContractPaymentPartDbService = platformContractPaymentPartDbService;

            _environment = environment;

            _infoValidator = infoValidator;
            _datesValidator = datesValidator;
            _completeValidator = completeValidator;
        }

        public async Task<PlatformContractAddResponseDto> AddPlatformContract(int userId)
        {
            PlatformContract contract = await _platformContractDbService.Add(new PlatformContract
            {
                UserId = userId,
                Status = PlatformContractStatuses.Draft,
                VersionNumber = 1,
                VersionStatus = PlatformContractVersionStatuses.Actual,
                CreatedDate = DateTime.UtcNow
            });

            return new PlatformContractAddResponseDto
            {
                ContractId = contract.Id
            };
        }

        private async Task<ResponseData> CheckContractExist(ResponseData model, int contractId)
        {
            if (!await _platformContractDbService.Any(contractId))
                model.AddError("Contract", "Contract doesn't exist!");
            return model;
        }

        private async Task<ResponseData> CheckCityExist(ResponseData model, int cityId)
        {
            if (!await _platformCityDbService.Any(cityId))
                model.AddError("City", "City doesn't exist!");
            return model;
        }

        private async Task<ResponseData> CheckPriorityExist(ResponseData model, int priorityId)
        {
            if (!await _platformContractPriorityDbService.Any(priorityId))
                model.AddError("Priority", "Priority doesn't exist!");
            return model;
        }

        private async Task<ResponseData> CheckTypeExist(ResponseData model, int typeId)
        {
            if (!await _platformContractTypeDbService.Any(typeId))
                model.AddError("Type", "Type doesn't exist!");
            return model;
        }

        private async Task<ResponseData> CheckBalanceExist(ResponseData model, int balanceId)
        {
            if (!await _platformBalanceDbService.Any(balanceId))
                model.AddError("Balance", "Balance doesn't exist!");
            return model;
        }

        private async Task<ResponseData> CheckCategoriesExist(ResponseData model, List<int> categoryIds)
        {
            if (!await _platformCategoryDbService.AllExists(categoryIds))
                model.AddError("Categories", "Categories doesn't exist!");

            return model;
        }

        private async Task<ResponseData> CheckUserContract(ResponseData model, int contractId, int userId)
        {
            if (!await _platformContractDbService.Any(contractId, userId))
                model.AddError("User", "Access denied!");
            return model;
        }

        public async Task<ResponseData> UpdatePlatformContractInfo(
            int userId, int contractId, PlatformContractInfoDto request)
        {
            // Validation

            ResponseData response = await CheckContractExist(new ResponseData { }, contractId);

            if (response.Errors.Count != 0)
                return response;

            ValidationResult validationResult = _infoValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                response.Errors = validationResult.ToErrors();
                return response;
            }

            response = await CheckCityExist(response, request.CityId.Value);
            response = await CheckPriorityExist(response, request.PriorityId.Value);
            response = await CheckTypeExist(response, request.ContractTypeId.Value);
            response = await CheckUserContract(response, contractId, userId);
            response = await CheckCategoriesExist(response, request.CategoryIds);

            PlatformContract contract = await _platformContractDbService.Get(contractId);

            if (contract.VersionStatus == PlatformContractVersionStatuses.NotActual)
                response.AddError("VersionStatus", "Contract is not actual!");
            if (contract.Status != PlatformContractStatuses.Draft)
                response.AddError("Status", "You can edit contract only in draft mode!");

            if (response.Errors.Count != 0)
                return response;

            // End validation

            contract.Name = request.Name;
            contract.Address = request.Address;
            contract.AddressHidden = request.AddressHidden;
            contract.ContractTypeId = request.ContractTypeId;
            contract.ContractPriorityId = request.PriorityId;
            contract.CityId = request.CityId;
            contract.UpdatedDate = DateTime.UtcNow;
            contract.EditStatus = PlatformContractEditStatuses.Info;

            await _platformContractDbService.Update(contract);

            // Categories
            List<PlatformContractCategory> existsContractCategories = await _platformContractCategoryDbService.List(contract.Id);
            List<PlatformCategory> requestCategories = await _platformCategoryDbService.List(request.CategoryIds);
            List<PlatformCategory> existsCategories = existsContractCategories.Select(c => c.PlatformCategory).ToList();
            List<PlatformCategory> categoriesForRemove = existsCategories.Except(requestCategories).ToList();

            await _platformContractCategoryDbService.RemoveRange(
                existsContractCategories.Where(c => categoriesForRemove.Any(cr => cr.Id == c.PlatformCategoryId)));
            await _platformContractCategoryDbService.AddRange(contractId,
                requestCategories.Except(existsCategories).Select(c => c.Id).ToList());

            // Tags
            List<PlatformTag> tags = await _platformTagDbService.List(request.Tags);
            await _platformTagDbService.AddRange(request.Tags.Where(t => !tags.Any(et => et.Name == t))
                .Select(t => new PlatformTag { Name = t }));
            tags = await _platformTagDbService.List(request.Tags);

            List<PlatformContractTag> existsContractTags = await _platformContractTagDbService.List(contract.Id);
            await _platformContractTagDbService.RemoveRange(existsContractTags.Where(e => !tags.Any(t => t.Name == e.PlatformTag.Name)));
            await _platformContractTagDbService.AddRange(tags.Where(t => !existsContractTags.Any(e => e.PlatformTag.Name == t.Name))
                .Select(t => new PlatformContractTag { PlatformContractId = contract.Id, PlatformTagId = t.Id }));

            return response;
        }

        public async Task<ResponseData> UpdatePlatformContractEstimate(
            int userId, int contractId, PlatformContractEstimateDto request)
        {
            // Validation

            ResponseData response = await CheckContractExist(new ResponseData { }, contractId);
            response = await CheckUserContract(response, contractId, userId);

            if (response.Errors.Count != 0)
                return response;

            PlatformContract contract = await _platformContractDbService.Get(contractId);
            PlatformEstimate estimate = await _platformEstimateDbService.Get(contract.Id, contract.UserId);
            int estimateId = 0;

            if (contract.VersionStatus == PlatformContractVersionStatuses.NotActual)
                response.AddError("VersionStatus", "Contract is not actual!");
            if (contract.Status != PlatformContractStatuses.Draft)
                response.AddError("Status", "You can edit contract only in draft mode!");

            if (response.Errors.Count != 0)
                return response;

            // End validation

            if (estimate != null && estimate.FileName == request.FileName)
            {
                estimate.Name = request.Name;
                estimate.IsOrdered = request.IsOrdered;
                await _platformEstimateDbService.Update(estimate);
                estimateId = estimate.Id;
            }
            else
            {
                if (estimate != null)
                    await _platformEstimateService.Remove(estimate);

                try
                {
                    estimateId = await _platformEstimateService.ParseAdd(request.Name, request.FileName, request.IsOrdered, contract.Id, contract.UserId, 1);
                }
                catch (Exception e)
                {
                    response.AddError("Estimate", e.ToString());
                }
            }

            // Update description
            contract.EditStatus = PlatformContractEditStatuses.Estimate;
            contract.UpdatedDate = DateTime.UtcNow;
            contract.Description = request.Description;
            contract.MainEstimateId = estimateId;
            await _platformContractDbService.Update(contract);

            List<PlatformContractFile> existFiles = await _platformContractFileDbService.List(contract.Id);
            List<PlatformContractFile> filesToRemove = existFiles.Where(ef => !request.Files.Any(f => ef.FileName == f)).ToList();
            List<PlatformContractFile> filesToAdd = request.Files.Where(f => !existFiles.Any(ef => ef.FileName == f))
                .Select(f => new PlatformContractFile
                {
                    FileName = f,
                    PlatformContractId = contract.Id
                }).ToList();

            // Remove old files
            foreach (PlatformContractFile file in filesToRemove)
            {
                string path = Path.Combine(_environment.WebRootPath, "docs", "src", file.FileName);
                if (File.Exists(path))
                    File.Delete(path);
            }

            await _platformContractFileDbService.RemoveRange(filesToRemove);

            // Add new files
            for (int i = 0; i < filesToAdd.Count; i++)
            {
                string tempPath = Path.Combine(_environment.WebRootPath, "docs", "temp", filesToAdd[i].FileName);
                string srcPath = Path.Combine(_environment.WebRootPath, "docs", "src", filesToAdd[i].FileName);

                if (File.Exists(tempPath))
                {
                    File.Move(tempPath, srcPath);
                }
                else
                {
                    filesToAdd.RemoveAt(i);
                    i--;
                }
            }

            await _platformContractFileDbService.AddRange(filesToAdd);

            return response;
        }

        public async Task<ResponseData> UpdatePlatformContractPrice(
            int userId, int contractId, PlatformContractPriceDto request)
        {
            // Validation

            ResponseData response = await CheckContractExist(new ResponseData { }, contractId);
            response = await CheckUserContract(response, contractId, userId);
            response = await CheckBalanceExist(response, request.BalanceId);

            if (response.Errors.Count != 0)
                return response;

            PlatformContract contract = await _platformContractDbService.Get(contractId);

            if (contract.VersionStatus == PlatformContractVersionStatuses.NotActual)
                response.AddError("VersionStatus", "Contract is not actual!");
            if (contract.Status != PlatformContractStatuses.Draft)
                response.AddError("Status", "You can edit contract only in draft mode!");

            if (response.Errors.Count != 0)
                return response;

            // End validation

            contract.Price = request.Price;
            contract.BalanceId = request.BalanceId;
            contract.EditStatus = PlatformContractEditStatuses.Price;
            contract.UpdatedDate = DateTime.UtcNow;
            await _platformContractDbService.Update(contract);

            return response;
        }

        public async Task<ResponseData> UpdatePlatformContractDates(
            int userId, int contractId, PlatformContractDatesDto request)
        {
            // Validation

            ResponseData response = await CheckContractExist(new ResponseData { }, contractId);
            response = await CheckUserContract(response, contractId, userId);

            if (response.Errors.Count != 0)
                return response;

            PlatformContract contract = await _platformContractDbService.Get(contractId);

            if (contract.VersionStatus == PlatformContractVersionStatuses.NotActual)
                response.AddError("VersionStatus", "Contract is not actual!");
            if (contract.Status != PlatformContractStatuses.Draft)
                response.AddError("Status", "You can edit contract only in draft mode!");

            ValidationResult validationResult = _datesValidator.Validate(request);

            if (!validationResult.IsValid)
            {
                response.Errors = validationResult.ToErrors();
                return response;
            }

            // End validation

            contract.PublishDate = request.PublishDate;
            contract.TenderEndDate = request.TenderEndDate;
            contract.PlannedBuildEnd = request.PlannedBuildEnd;
            contract.BuildStart = request.BuildStart;
            contract.EditStatus = PlatformContractEditStatuses.Dates;
            contract.UpdatedDate = DateTime.UtcNow;

            await _platformContractDbService.Update(contract);

            return response;
        }

        public async Task<ResponseData> CompletePlatformContract(
            int userId, int contractId)
        {
            // Validation

            ResponseData response = await CheckContractExist(new ResponseData(), contractId);
            response = await CheckUserContract(response, contractId, userId);

            if (response.Errors.Count != 0)
                return response;

            PlatformContract contract = await _platformContractDbService.Get(contractId);

            if (contract.VersionStatus == PlatformContractVersionStatuses.NotActual)
                response.AddError("VersionStatus", "Contract is not actual!");
            if (contract.Status != PlatformContractStatuses.Draft)
                response.AddError("Status", "You can edit contract only in draft mode!");

            ValidationResult validationResult = _completeValidator.Validate(contract);

            if (!validationResult.IsValid)
            {
                response.Errors = validationResult.ToErrors();
                return response;
            }

            response = await CheckCityExist(new ResponseData { }, contract.CityId.Value);
            response = await CheckPriorityExist(response, contract.ContractPriorityId.Value);

            if (response.Errors.Count != 0)
                return response;

            // End validation

            if (contract.PublishDate <= DateTime.UtcNow)
                contract.Status = PlatformContractStatuses.Contractor;
            else
                contract.Status = PlatformContractStatuses.Deferred;

            contract.UpdatedDate = DateTime.UtcNow;

            await _platformContractDbService.Update(contract);

            return response;
        }

        public async Task<PaginatedList<PlatformContractDto>> GetPaginatedList(int userId, int skip, int take,
            string filter, List<string> statuses, SortType sort, bool isActual)
        {
            return new PaginatedList<PlatformContractDto>
            {
                List = (await _platformContractDbService.List(userId, skip, take, filter, statuses, sort, isActual)).Select(c => new PlatformContractDto
                {
                    Id = c.Id,
                    OriginalId = c.OriginalId,
                    Type = c.ContractType?.Name,
                    Name = c.Name,
                    Categories = c.Categories.Select(c => c.PlatformCategory.NameId).ToList(),
                    PublishDate = c.PublishDate,
                    PlannedBuildEnd = c.PlannedBuildEnd,
                    EditStatus = c.EditStatus,
                    Version = c.VersionNumber,
                    VersionStatus = c.VersionStatus,
                    Status = c.Status,
                    CreatedDate = c.CreatedDate,
                    UpdatedDate = c.UpdatedDate,
                    UnreadMessagesCount = c.PlatformChatRooms.Count() > 0 ? _platformChatRoomMessageDbService.UnreadCount(c.PlatformChatRooms.FirstOrDefault().Id, userId) : 0,
                    ChatRoomId = c.PlatformChatRooms.Count() > 0 ? c.PlatformChatRooms.FirstOrDefault().Id : 0
                }).ToList(),
                CountAll = await _platformContractDbService.Count(userId, filter, statuses)
            };
        }

        public async Task<PlatformContractFullDto> Get(int userId, int contractId)
        {
            PlatformContract contract = await _platformContractDbService.GetFull(contractId);
            if (contract.Status == PlatformContractStatuses.Draft && contract.UserId != userId)
                return null;

            PlatformEstimate estimate = contract.PlatformEstimates.FirstOrDefault(e => e.CreatorId == contract.UserId);

            return new PlatformContractFullDto
            {
                Id = contract.Id,
                Address = contract.Address,
                AddressHidden = contract.AddressHidden,
                BuildStart = contract.BuildStart,
                CityId = contract.CityId,
                CountryId = contract.City?.CountryId,
                ContractPriorityId = contract.ContractPriorityId,
                ContractType = contract.ContractType?.Name,
                ContractTypeId = contract.ContractTypeId,
                Name = contract.Name,
                EstimateId = estimate?.Id,
                EstimateName = estimate?.Name,
                EstimateFileName = estimate?.FileName,
                IsEstimateOrdered = estimate?.IsOrdered,
                MainEstimateId = contract.MainEstimateId,
                Description = contract.Description,
                PlannedBuildEnd = contract.PlannedBuildEnd,
                Price = contract.Price,
                PublishDate = contract.PublishDate,
                Status = contract.Status,
                TenderEndDate = contract.TenderEndDate,
                Categories = contract.Categories.Select(c => c.PlatformCategory.Id).ToList(),
                Tags = contract.Tags.Select(c => c.PlatformTag.Name).ToList(),
                Files = contract.Files.Select(f => f.FileName).ToList(),
                BalanceId = contract.BalanceId,
                EditStatus = contract.EditStatus,
                CreatedDate = contract.CreatedDate,
                UpdateDate = contract.UpdatedDate,
                IsPrepayment = contract.IsPrepayment,
                PrepaymentValue = contract.PrepaymentValue,
                IsPrepaymentMade = contract.IsPrepaymentMade,
                IsPrepaymentPaid = contract.IsPrepaymentPaid,
                PaymentType = contract.PaymentType,
                PaymentFrequency = contract.PaymentFrequency,
                IsReservationPaid = contract.IsReservationPaid,
                IsReservationMade = contract.IsResevationMade,
                SumOfReservation = contract.PaymentType == PlatformContractPaymentTypes.Safe ? (contract.Categories.Select(c => c.PlatformCategory.ReservationPercent).Max() / 100 * contract.Price) : 0.0,
                BudgetOfWorks = contract.BudgetOfWorks,
                HoursOfWorks = contract.HoursOfWorks,
                ComissionType = contract.ComissionType,
                CooperationType = contract.CooperationType,
                CostOfHour = contract.CostOfHour
            };
        }

        public async Task<PlatformContractProgressDto> GetExecutorDto(int userId, int contractId)
        {
            PlatformContract contract = await _platformContractDbService.GetFull(contractId);
            if (contract.Status == PlatformContractStatuses.Draft)
                return null;

            PlatformContractBid acceptedBid = await _platformContractBidDbService.GetAccepted(contractId);
            PlatformContractChange lastChange = await _platformContractChangeDbService.GetLast(contractId);

            return new PlatformContractProgressDto
            {

                ExecutorId = acceptedBid.ApplicationUserId,
                LastChanged = lastChange == null ? null :
                    contract.UserId == lastChange.ApplicationUserId ? "Customer" :
                    acceptedBid != null && acceptedBid.ApplicationUserId == lastChange.ApplicationUserId ? "Executor" : null,
                ContractCustomer = contract.Approval.ContractCustomer,
                ContractExecutor = contract.Approval.ContractExecutor,
                CustomContractText = contract.Approval.CustomContractText,
                EstimateCustomer = contract.Approval.EstimateCustomer,
                EstimateExecutor = contract.Approval.EstimateExecutor,
                FinishCustomer = contract.Approval.FinishCustomer,
                FinishExecutor = contract.Approval.FinishExecutor,
                SigningCustomer = contract.Approval.SigningCustomer,
                SigningExecutor = contract.Approval.SigningExecutor
            };
        }

        public async Task<PlatformContractContractorDto> GetContractorDto(int userId, int contractId)
        {
            ResponseData response = await CheckContractExist(new ResponseData { }, contractId);

            PlatformContract contract = await _platformContractDbService.GetFull(contractId);

            if (contract.Status == PlatformContractStatuses.Draft)
                return null;

            PlatformEstimate estimate = contract.PlatformEstimates.FirstOrDefault(e => e.CreatorId == contract.UserId);
            List<PlatformContractReview> reviews = await _platformContractReviewDbService.List(contract.UserId);

            return new PlatformContractContractorDto
            {
                Id = contract.Id,
                BuildStart = contract.BuildStart,
                ContractType = contract.ContractType?.Name,
                Name = contract.Name,
                EstimateFileName = estimate?.FileName,
                EstimateName = estimate?.Name,
                Description = contract.Description,
                PlannedBuildEnd = contract.PlannedBuildEnd,
                TenderEndDate = contract.TenderEndDate,
                Files = contract.Files.Select(f => f.FileName).ToList(),
                ViewsCount = await _platformContractViewDbService.Count(contractId),
                PublishDate = contract.PublishDate,
                UserId = contract.User.Id,
                CreatedDate = contract.CreatedDate,
                UpdatedDate = contract.UpdatedDate,
                EstimateId = estimate?.Id,
                IsFavorite = await _platformContractFavoriteDbService.Any(userId, contractId)
            };
        }

        private void RemoveFiles(PlatformContract contract)
        {
            foreach (PlatformEstimate estimate in contract.PlatformEstimates)
            {
                string estimatePath = Path.Combine(_environment.WebRootPath, "docs", "src", estimate.FileName);

                if (File.Exists(estimatePath))
                    File.Delete(estimatePath);
            }

            foreach (PlatformContractFile file in contract.Files)
            {
                string filePath = Path.Combine(_environment.WebRootPath, "docs", "src", file.FileName);

                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
        }

        public async Task<ResponseData> Remove(int userId, int id)
        {
            // Validation

            ResponseData response = await CheckContractExist(new ResponseData(), id);
            response = await CheckUserContract(response, id, userId);

            if (response.Errors.Count != 0)
                return response;

            // End validation

            PlatformContract contract = await _platformContractDbService.GetFull(id);

            if (contract.Status == PlatformContractStatuses.Draft)
            {
                RemoveFiles(contract);
                await _platformContractDbService.Remove(contract);
            }
            else
            {
                contract.Status = PlatformContractStatuses.Archived;
                await _platformContractDbService.Update(contract);
            }

            return response;
        }

        public async Task<ResponseData<PlatformContractCloneResponseDto>> Clone(int userId, PlatformContractCloneDto request)
        {
            // Validation

            ResponseData response = await CheckContractExist(new ResponseData { }, request.Id);
            response = await CheckUserContract(response, request.Id, userId);

            if (response.Errors.Count != 0)
                return new ResponseData<PlatformContractCloneResponseDto> { Errors = response.Errors };

            // End validation

            PlatformContract newContract = await _platformContractCloneService.Clone(request.Id);

            return new ResponseData<PlatformContractCloneResponseDto>
            {
                Data = new PlatformContractCloneResponseDto { Id = newContract.Id }
            };
        }

        public async Task<PaginatedList<PlatformContractDto>> GetPaginatedListVersions(int userId, int contractId, int skip, int take)
        {
            // Validation

            ResponseData response = await CheckContractExist(new ResponseData { }, contractId);
            response = await CheckUserContract(response, contractId, userId);

            if (response.Errors.Count != 0)
                return null;

            // End validation

            return new PaginatedList<PlatformContractDto>
            {
                CountAll = await _platformContractDbService.CountVersions(contractId),
                List = (await _platformContractDbService.ListVersions(contractId, skip, take))
                    .Select(c => new PlatformContractDto
                    {
                        Id = c.Id,
                        Type = c.ContractType?.Name,
                        Name = c.Name,
                        Categories = c.Categories.Select(c => c.PlatformCategory.NameId).ToList(),
                        PublishDate = c.PublishDate,
                        PlannedBuildEnd = c.PlannedBuildEnd,
                        EditStatus = c.EditStatus,
                        Version = c.VersionNumber,
                        VersionStatus = c.VersionStatus,
                        Status = c.Status,
                    }).ToList()
            };
        }

        public async Task<ResponseData> SetTenderEnd(int userId, int contractId, PlatformContractTenderEndRequestDto request)
        {
            // Validation

            ResponseData response = await CheckContractExist(new ResponseData { }, contractId);
            response = await CheckUserContract(response, contractId, userId);

            if (response.Errors.Count != 0)
                return null;

            // End validation

            PlatformContract contract = await _platformContractDbService.Get(contractId);
            contract.TenderEndDate = request.TenderEnd;
            await _platformContractDbService.Update(contract);

            return new ResponseData();
        }

        public async Task<ResponseData> Archive(int userId, int id)
        {
            // Validation

            ResponseData response = await CheckContractExist(new ResponseData { }, id);
            response = await CheckUserContract(response, id, userId);

            if (response.Errors.Count != 0)
                return null;

            // End validation

            PlatformContract contract = await _platformContractDbService.Get(id);
            contract.Status = PlatformContractStatuses.Archived;
            await _platformContractDbService.Update(contract);

            return new ResponseData();
        }

        public async Task<ResponseData> Refuse(int userId, int contractId, PlatformContractRefuseRequestDto request)
        {
            if (!await _platformContractDbService.IsCustomerOrExecutor(contractId, userId))
                return ResponseData.Fail("User", "Access denied!");

            PlatformContract contract = await _platformContractDbService.Get(contractId);

            if (contract.Status != PlatformContractStatuses.Refusal)
            {
                contract.Status = PlatformContractStatuses.Refusal;
                contract.RefusalCase = request.RefusalCase;

                await _platformContractDbService.Update(contract);

                await _platformContractChangeDbService.Add(new PlatformContractChange(userId, contractId, PlatformContractChangeCases.Refuse,
                    false.ToString(), request.RefusalCase, DateTime.UtcNow));
            }

            return ResponseData.Ok();
        }

        public async Task<ResponseData> Close(int userId, int contractId, PlatformContractCloseRequestDto request)
        {
            if (!await _platformContractDbService.IsCustomerOrExecutor(contractId, userId))
                return ResponseData.Fail("User", "Access denied!");

            PlatformContract contract = await _platformContractDbService.Get(contractId);

            if (contract.Status != PlatformContractStatuses.Closed)
            {
                contract.Status = PlatformContractStatuses.Closed;
                contract.ClosingCase = request.ClosingCase;

                await _platformContractDbService.Update(contract);

                await _platformContractChangeDbService.Add(new PlatformContractChange(userId, contractId, PlatformContractChangeCases.Close,
                    false.ToString(), request.ClosingCase, DateTime.UtcNow));
            }

            return ResponseData.Ok();
        }

        public async Task<ResponseData> Finish(int userId, int contractId)
        {
            bool isCustomer = await _platformContractDbService.IsCustomer(contractId, userId);
            bool isExecutor = await _platformContractDbService.IsExecutor(contractId, userId);

            if (!isCustomer && !isExecutor)
                return ResponseData.Fail("User", "Access denied!");

            PlatformContract contract = await _platformContractDbService.GetApproval(contractId);

            if (isExecutor && !contract.Approval.FinishExecutor)
            {
                contract.Approval.FinishExecutor = true;
                if (contract.Approval.FinishCustomer)
                    contract.Status = PlatformContractStatuses.Finished;

                await _platformContractDbService.Update(contract);

                await _platformContractChangeDbService.Add(new PlatformContractChange(userId, contractId, PlatformContractChangeCases.Finish,
                    false.ToString(), true.ToString(), DateTime.UtcNow));
            }
            else if (isCustomer && !contract.Approval.FinishCustomer)
            {
                contract.Approval.FinishCustomer = true;
                if (contract.Approval.FinishExecutor)
                    contract.Status = PlatformContractStatuses.Finished;

                await _platformContractDbService.Update(contract);

                await _platformContractChangeDbService.Add(new PlatformContractChange(userId, contractId, PlatformContractChangeCases.Finish,
                    false.ToString(), true.ToString(), DateTime.UtcNow));
            }

            return new ResponseData();
        }

        public async Task<PaginatedList<PlatformContractPublishedDto>> GetPublished(bool hideMyBids, int? balanceId, string filter, SortType sort, List<int> categories, List<string> tags, int userId, int skip, int take)
        {
            return new PaginatedList<PlatformContractPublishedDto>
            {
                List = (await _platformContractDbService.ListPublished(
                    userId, skip, take, filter, categories, tags, hideMyBids, balanceId, sort))
                .Select(c => new PlatformContractPublishedDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    PublishDate = c.PublishDate,
                    TenderEndDate = c.TenderEndDate,
                    Price = c.Price,
                    BalanceId = c.BalanceId,
                    FullName = $"{c.User.Name} {c.User.Surname}",
                    UserAvatar = c.User.Avatar,
                    Rating = c.User.Ratings.Sum(r => r.Points),
                    BadReviews = c.User.Reviews.Count(r => r.Marks.Average(r => r.Value) >= 2.5),
                    GoodReviews = c.User.Reviews.Count(r => r.Marks.Average(r => r.Value) < 2.5),
                    EstimateName = c.PlatformEstimates.FirstOrDefault(e => e.CreatorId == c.UserId)?.Name,
                    EstimateFileName = c.PlatformEstimates.FirstOrDefault(e => e.CreatorId == c.UserId)?.FileName,
                    EstimateId = c.PlatformEstimates.FirstOrDefault(e => e.CreatorId == c.UserId)?.Id,
                    CountDiscussions = c.Discussions.Count(),
                    CountBids = c.Bids.Count(),
                    CreatedDate = c.CreatedDate,
                    UpdatedDate = c.UpdatedDate
                }).ToList(),
                CountAll = await _platformContractDbService.CountPublished(
                    userId, filter, categories, tags, hideMyBids, balanceId)
            };
        }

        public async Task<PlatformContractExecutionDto> GetExecution(int userId, int contractId)
        {
            var result = new PlatformContractExecutionDto();
            PlatformContract contract = await _platformContractDbService.Get(contractId);
            if (contract.BuildStart != null && contract.PlannedBuildEnd != null)
            {
                if (DateTime.Now > contract.BuildStart && DateTime.Now <= contract.PlannedBuildEnd)
                {
                    var term = (contract.PlannedBuildEnd - contract.BuildStart).Value.TotalDays;
                    var fromStart = (DateTime.Now - contract.BuildStart).Value.TotalDays;
                    var tillEnd = (contract.PlannedBuildEnd - DateTime.Now).Value.TotalDays;

                    result.Term = fromStart / term * 100;
                    result.TermLeft = tillEnd / term * 100;
                }
                else if (DateTime.Now < contract.BuildStart)
                {
                    result.Term = 0;
                    result.TermLeft = 100;
                }
                else
                {
                    result.Term = 100;
                    result.TermLeft = 0;
                }
            }

            if (contract.CooperationType == PlatformContractCooperationType.IndependentMeasurements)
            {
                var mReports = await _platformMeasurementReportDbService.List(contractId);
                if (mReports.Count > 0)
                {
                    result.PaidPercent = (double)(mReports.Where(m => m.IsPaid == true).Sum(m => m.PriceWithVat) / contract.Price) * 100;
                    result.PaidMoney = mReports.Where(m => m.IsPaid == true).Sum(m => m.PriceWithVat);

                    result.PaidMoneyLeft = (double)(contract.Price - result.PaidMoney);
                    result.PaidPercentLeft = 100 - result.PaidPercent;

                    result.Invoiced = (double)(mReports.Where(m => !string.IsNullOrEmpty(m.InvoiceFile)).Sum(m => m.PriceWithVat) / contract.Price) * 100;
                    result.InvoicedLeft = 100 - result.Invoiced;

                    var mPositions = await _platformMeasurementReportPositionDbService.List(mReports.First().Id);

                    if (mPositions.Count > 0)
                    {
                        result.Works = (double)mPositions.Average(p => p.PercentCompleted);
                        result.WorksLeft = 100 - result.Works;
                    }
                }
            }
            if (contract.CooperationType == PlatformContractCooperationType.ForStagesOfWork)
            {
                var paymentParts = await _platformContractPaymentPartDbService.List(contractId);
                if (paymentParts.Count > 0)
                {
                    result.PaidPercent = (double)(paymentParts.Where(m => m.IsPaid == true).Sum(m => m.Value) / contract.Price) * 100;
                    result.PaidMoney = paymentParts.Where(m => m.IsPaid == true).Sum(m => m.Value);

                    result.PaidMoneyLeft = (double)(contract.Price - result.PaidMoney);
                    result.PaidPercentLeft = 100 - result.PaidPercent;

                    result.Invoiced = (double)(paymentParts.Where(m => !string.IsNullOrEmpty(m.InvoiceFile)).Sum(m => m.Value) / contract.Price) * 100;
                    result.InvoicedLeft = 100 - result.Invoiced;

                    result.Works = result.Invoiced;
                    result.WorksLeft = 100 - result.Works;
                }
            }

            return result;
        }
    }
}
