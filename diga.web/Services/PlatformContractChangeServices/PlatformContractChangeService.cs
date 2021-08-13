using diga.bl.Constants;
using diga.bl.Models;
using diga.bl.Models.Platform.Contracts;
using diga.bl.Models.Platform.Estimates;
using diga.dal.DbServices.PlatformContractApprovalDbServices;
using diga.dal.DbServices.PlatformContractBidDbServices;
using diga.dal.DbServices.PlatformContractChangeDbServices;
using diga.dal.DbServices.PlatformContractDbServices;
using diga.dal.DbServices.PlatformContractPaymentPartDbServices;
using diga.dal.DbServices.PlatformEstimateDbServices;
using diga.dal.DbServices.UserDbServices;
using diga.web.Models.PlatformContractChanges;
using diga.web.Models.PlatformNotifications;
using diga.web.Models.Status;
using diga.web.Services.PlatformNotificationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static diga.bl.Constants.PlatformContractChangeCases;

namespace diga.web.Services.PlatformContractChangeServices
{
    public class PlatformContractChangeService : IPlatformContractChangeService
    {
        private readonly IPlatformContractChangeDbService _platformContractChangeDbService;
        private readonly IPlatformContractPaymentPartDbService _platformContractPaymentPartDbService;
        private readonly IPlatformContractDbService _platformContractDbService;
        private readonly IPlatformContractApprovalDbService _platformContractApprovalDbService;
        private readonly IPlatformContractBidDbService _platformContractBidDbService;
        private readonly IPlatformEstimateDbService _platformEstimateDbService;
        private readonly IPlatformNotificationService _platformNotificationService;
        private readonly IUserDbService _userDbService;

        public PlatformContractChangeService(IPlatformContractChangeDbService platformContractChangeDbService,
            IPlatformContractPaymentPartDbService platformContractPaymentPartDbService,
            IPlatformContractDbService platformContractDbService,
            IPlatformContractApprovalDbService platformContractApprovalDbService,
            IPlatformContractBidDbService platformContractBidDbService,
            IPlatformEstimateDbService platformEstimateDbService,
            IPlatformNotificationService platformNotificationService,
            IUserDbService userDbService)
        {
            _platformContractChangeDbService = platformContractChangeDbService;
            _platformContractPaymentPartDbService = platformContractPaymentPartDbService;
            _platformContractDbService = platformContractDbService;
            _platformContractApprovalDbService = platformContractApprovalDbService;
            _platformContractBidDbService = platformContractBidDbService;
            _platformEstimateDbService = platformEstimateDbService;
            _platformNotificationService = platformNotificationService;
            _userDbService = userDbService;
        }

        public async Task<ResponseData> Approve(int userId, int contractId)
        {
            bool isCustomer = await _platformContractDbService.IsCustomer(contractId, userId);
            bool isExecutor = await _platformContractDbService.IsExecutor(contractId, userId);
            PlatformContract contract = await _platformContractDbService.Get(contractId);

            if (contract.Status != PlatformContractStatuses.ContractApproval)
                return ResponseData.Fail("Status", "Contract can be approved only after estimate approval!");

            if (!isCustomer && !isExecutor)
                return ResponseData.Fail("User", "Access denied!");

            PlatformContractApproval approval = await _platformContractApprovalDbService.Get(contractId);

            if (!approval.EstimateCustomer || !approval.EstimateExecutor)
                return ResponseData.Fail("Estimate", "Estimate not approved!");
            if (isExecutor && !approval.ContractCustomer)
                return ResponseData.Fail("CustomerAccepted", "Customer must accepted contract!");

            if (isExecutor && !approval.ContractExecutor)
            {
                approval.ContractExecutor = true;
                await _platformContractApprovalDbService.Update(approval);

                DateTime now = DateTime.UtcNow;
                await _platformContractChangeDbService.Add(new PlatformContractChange(userId, contractId, ApproveContract,
                    false.ToString(), true.ToString(), now));

                ApplicationUser user = await _userDbService.Get(userId);
                await _platformNotificationService.AddNotification(contract.UserId, new PlatformNotificationDto
                {
                    IsRead = false,
                    DateTime = now,
                    Type = PlatformNotificationTypes.ExecutorApproved,
                    Datas = new List<PlatformNotificationDataDto>
                    {
                        new PlatformNotificationDataDto("ContractId", contractId.ToString()),
                        new PlatformNotificationDataDto("ExecutorName", user.Name)
                    }
                });
            }
            else if (isCustomer && !approval.ContractCustomer)
            {
                approval.ContractCustomer = true;
                await _platformContractApprovalDbService.Update(approval);

                await _platformContractChangeDbService.Add(new PlatformContractChange(userId, contractId, ApproveContract,
                    false.ToString(), true.ToString(), DateTime.UtcNow));
            }

            if (approval.ContractCustomer && approval.ContractExecutor)
            {
                contract.Status = PlatformContractStatuses.Signing;
                await _platformContractDbService.Update(contract);
            }

            return new ResponseData();
        }

        public async Task<ResponseData> Edit(int userId, int contractId, PlatformContractChangeInfoDto request)
        {
            bool isCustomer = await _platformContractDbService.IsCustomer(contractId, userId);
            bool isExecutor = await _platformContractDbService.IsExecutor(contractId, userId);

            if (!isCustomer && !isExecutor)
                return ResponseData.Fail("User", "Access denied!");

            PlatformContract contract = await _platformContractDbService.Get(contractId);
            List<PlatformContractChange> changes = new List<PlatformContractChange> { };
            DateTime now = DateTime.UtcNow;

            // Build start
            if (request.BuildStart != null && request.BuildStart != contract.BuildStart)
            {
                changes.Add(new PlatformContractChange(userId, contractId, BuildStart,
                    contract.BuildStart.ToString(), request.BuildStart.ToString(), now));

                contract.BuildStart = request.BuildStart;
            }

            // Planned build end
            if (request.PlannedBuildEnd != null && request.PlannedBuildEnd != contract.PlannedBuildEnd)
            {
                changes.Add(new PlatformContractChange(userId, contractId, BuildStart,
                    contract.PlannedBuildEnd.ToString(), request.PlannedBuildEnd.ToString(), now));

                contract.PlannedBuildEnd = request.PlannedBuildEnd;
            }

            // Price
            if (request.Price != null && request.Price != contract.Price)
            {
                changes.Add(new PlatformContractChange(userId, contractId, Price,
                    contract.Price.ToString(), request.Price.ToString(), now));

                contract.Price = request.Price;
            }

            // Description
            if (!string.IsNullOrEmpty(request.Description) && request.Description != contract.Description)
            {
                changes.Add(new PlatformContractChange(userId, contractId, Description,
                    contract.Description, request.Description, now));

                contract.Description = request.Description;
            }

            // PaymentType
            if (!string.IsNullOrEmpty(request.PaymentType) && request.PaymentType != contract.PaymentType)
            {
                if (request.PaymentType != PlatformContractPaymentTypes.Direct && request.PaymentType != PlatformContractPaymentTypes.Safe)
                {
                    return ResponseData.Fail("PaymentType", "Incorrect!");
                }
                changes.Add(new PlatformContractChange(userId, contractId, PaymentType,
                    contract.PaymentType, request.PaymentType, now));

                contract.PaymentType = request.PaymentType;
            }

            // ComissionType
            if (!string.IsNullOrEmpty(request.ComissionType) && request.ComissionType != contract.ComissionType)
            {
                if (request.ComissionType != PlatformContractComissionTypes.InHalf && request.ComissionType != PlatformContractComissionTypes.PaidByCustomer && request.ComissionType != PlatformContractComissionTypes.PaidByExecutor)
                {
                    return ResponseData.Fail("ComissionType", "Incorrect!");
                }
                changes.Add(new PlatformContractChange(userId, contractId, ComissionType,
                    contract.ComissionType, request.ComissionType, now));

                contract.ComissionType = request.ComissionType;
            }

            //  CooperationType
            if (!string.IsNullOrEmpty(request.CooperationType) && request.CooperationType != contract.CooperationType)
            {
                if (request.CooperationType != PlatformContractCooperationType.ForHours && request.CooperationType != PlatformContractCooperationType.IndependentMeasurements && request.CooperationType != PlatformContractCooperationType.ForStagesOfWork)
                {
                    return ResponseData.Fail("CooperationType", "Incorrect!");
                }
                changes.Add(new PlatformContractChange(userId, contractId, CooperationType,
                    contract.CooperationType, request.CooperationType, now));

                contract.CooperationType = request.CooperationType;
            }

            //  BudgetOfWorks
            if (request.BudgetOfWorks != contract.BudgetOfWorks)
            {
                if (contract.CooperationType != PlatformContractCooperationType.ForHours)
                {
                    return ResponseData.Fail("BudgetOfWorks", "Allowed only if CooperationType is ForHours!");
                }
                changes.Add(new PlatformContractChange(userId, contractId, BudgetOfWorks,
                    contract.BudgetOfWorks.ToString(), request.BudgetOfWorks.ToString(), now));

                contract.BudgetOfWorks = request.BudgetOfWorks;
            }

            //  HoursOfWorks
            if (request.HoursOfWorks != contract.HoursOfWorks)
            {
                if (contract.CooperationType != PlatformContractCooperationType.ForHours)
                {
                    return ResponseData.Fail("HoursOfWorks", "Allowed only if CooperationType is ForHours!");
                }
                changes.Add(new PlatformContractChange(userId, contractId, HoursOfWorks,
                    contract.HoursOfWorks.ToString(), request.HoursOfWorks.ToString(), now));

                contract.HoursOfWorks = request.HoursOfWorks;
            }

            //  PaymentFrequency
            if (!string.IsNullOrEmpty(request.PaymentFrequency) && request.PaymentFrequency != contract.PaymentFrequency)
            {
                if (request.PaymentFrequency != PlatformContractPaymentFrequencies.Monthly 
                    && request.PaymentFrequency != PlatformContractPaymentFrequencies.Weekly 
                    && request.PaymentFrequency != PlatformContractPaymentFrequencies.OnceInTwoWeeks 
                    && request.PaymentFrequency != PlatformContractPaymentFrequencies.Daily)
                {
                    return ResponseData.Fail("PaymentFrequency", "Incorrect!");
                }
                changes.Add(new PlatformContractChange(userId, contractId, PaymentFrequency,
                    contract.PaymentFrequency, request.PaymentFrequency, now));

                contract.PaymentFrequency = request.PaymentFrequency;
            }

            //  General term
            if (request.GeneralTerm > 0 && request.GeneralTerm != contract.GeneralTerm)
            {
                changes.Add(new PlatformContractChange(userId, contractId, GeneralTerm,
                    contract.GeneralTerm.ToString(), request.GeneralTerm.ToString(), now));

                contract.GeneralTerm = request.GeneralTerm;
            }

            //  CostOfHour
            if (request.CostOfHour != contract.CostOfHour)
            {
                if (contract.CooperationType == PlatformContractCooperationType.ForHours)
                {
                    return ResponseData.Fail("CostOfHour", "Allowed only if CooperationType is ForHours!");
                }
                changes.Add(new PlatformContractChange(userId, contractId, CostOfHour,
                    contract.CostOfHour.ToString(), request.CostOfHour.ToString(), now));

                contract.CostOfHour = request.CostOfHour;
            }

            if (changes.Count != 0)
            {
                if (isCustomer)
                    contract.Approval.ContractExecutor = false;
                else if (isExecutor)
                    contract.Approval.ContractCustomer = false;

                await _platformContractDbService.Update(contract);
                await _platformContractChangeDbService.AddRange(changes);
            }

            PlatformContractBid acceptedBid = await _platformContractBidDbService.GetAccepted(contract.Id);
            await _platformNotificationService.AddNotification(
                contract.UserId == userId ? acceptedBid.ApplicationUserId : contract.UserId,
                new PlatformNotificationDto
                {
                    IsRead = false,
                    DateTime = DateTime.UtcNow,
                    Type = PlatformNotificationTypes.ContractChanged,
                    Datas = new List<PlatformNotificationDataDto> {
                        new PlatformNotificationDataDto("ContractId", contract.Id.ToString())
                    }
                });

            return new ResponseData();
        }

        private async Task UpdateParts(int userId, int contractId, List<PlatformContractPaymentPartDto> requestParts)
        {
            List<PlatformContractPaymentPart> parts = await _platformContractPaymentPartDbService.List(contractId);

            List<PlatformContractPaymentPart> partsToAdd = requestParts.Where(r => r.Id == 0)
                .Select(p => new PlatformContractPaymentPart
                {
                    Name = p.Name,
                    Number = p.Number,
                    PlatformContractId = contractId,
                    Value = p.Value,
                    Percent = p.Percent,
                    PercentOfWork = p.PercentOfWork
                }).ToList();
            List<PlatformContractPaymentPart> partsToUpdate = new List<PlatformContractPaymentPart> { };
            List<PlatformContractPaymentPart> partsToRemove = parts.Where(p => !requestParts.Any(rp => rp.Id == p.Id)).ToList();

            // Add changes
            DateTime now = DateTime.UtcNow;
            List<PlatformContractChange> changes = new List<PlatformContractChange> { };

            foreach (PlatformContractPaymentPart part in partsToAdd)
                changes.Add(new PlatformContractChange(userId, contractId, AddStep, string.Empty, string.Empty, now));

            foreach (PlatformContractPaymentPartDto requestPart in requestParts)
            {
                PlatformContractPaymentPart part = parts.FirstOrDefault(p => p.Id == requestPart.Id);

                if (part != null)
                {
                    bool isUpdate = false;

                    if (part.Name != requestPart.Name)
                    {
                        changes.Add(new PlatformContractChange(userId, contractId, StepName, part.Name, requestPart.Name, now));
                        part.Name = requestPart.Name;
                        isUpdate = true;
                    }

                    if (part.Value != requestPart.Value)
                    {
                        changes.Add(new PlatformContractChange(userId, contractId, StepValue, part.Value.ToString(), requestPart.Value.ToString(), now));
                        part.Value = requestPart.Value;
                        isUpdate = true;
                    }

                    if (part.DateTime != requestPart.DateTime)
                    {
                        changes.Add(new PlatformContractChange(userId, contractId, StepValue, part.DateTime.ToString(), requestPart.DateTime.ToString(), now));
                        part.DateTime = requestPart.DateTime;
                        isUpdate = true;
                    }

                    if (part.Number != requestPart.Number)
                    {
                        part.Number = requestPart.Number;
                        isUpdate = true;
                    }

                    if (part.PercentOfWork != requestPart.PercentOfWork)
                    {
                        part.PercentOfWork = requestPart.PercentOfWork;
                        isUpdate = true;
                    }

                    if (part.Percent != requestPart.Percent)
                    {
                        part.Percent = requestPart.Percent;
                        isUpdate = true;
                    }

                    if (isUpdate)
                        partsToUpdate.Add(part);
                }
            }

            foreach (PlatformContractPaymentPart part in partsToRemove)
                changes.Add(new PlatformContractChange(userId, contractId, RemoveStep, string.Empty, string.Empty, now));

            if (changes.Count != 0)
                await _platformContractChangeDbService.AddRange(changes);
            if (partsToAdd.Count != 0)
                await _platformContractPaymentPartDbService.AddRange(partsToAdd);
            if (partsToUpdate.Count != 0)
                await _platformContractPaymentPartDbService.UpdateRange(partsToUpdate);
            if (partsToRemove.Count != 0)
                await _platformContractPaymentPartDbService.RemoveRange(partsToRemove);
        }

        public async Task<ResponseData> EditPaymentParts(int userId, int contractId, PlatformContractChangePaymentPartsDto request)
        {
            if (!await _platformContractDbService.IsCustomer(contractId, userId))
                return ResponseData.Fail("User", "Access denied!");

            PlatformContract contract = await _platformContractDbService.Get(contractId);
            List<PlatformContractChange> changes = new List<PlatformContractChange> { };
            DateTime now = DateTime.UtcNow;

            if (contract.IsPrepayment != request.IsPrepayment)
            {
                changes.Add(new PlatformContractChange(userId, contractId, Prepayment,
                    contract.IsPrepayment.ToString(), request.IsPrepayment.ToString(), now));
                contract.IsPrepayment = request.IsPrepayment;                
            }

            if (contract.PrepaymentValue != request.PrepaymentValue)
            {
                if (contract.IsPrepayment != true)
                {
                    return ResponseData.Fail("PrepaymentValue", "Not allowed if IsPrepayment is not true!");
                }
                changes.Add(new PlatformContractChange(userId, contractId, PrepaymentValue,
                    contract.PrepaymentValue.ToString(), request.PrepaymentValue.ToString(), now));
                contract.PrepaymentValue = request.PrepaymentValue;
            }

            if (contract.PrepaymentPercent != request.PrepaymentPercent)
            {
                if (contract.IsPrepayment != true)
                {
                    return ResponseData.Fail("PrepaymentPercent", "Not allowed if IsPrepayment is not true!");
                }
                changes.Add(new PlatformContractChange(userId, contractId, PrepaymentPercent,
                    contract.PrepaymentPercent.ToString(), request.PrepaymentPercent.ToString(), now));
                contract.PrepaymentPercent = request.PrepaymentPercent;
            }

            if (changes.Count > 0)
            {
                await _platformContractDbService.Update(contract);
            }

            await UpdateParts(userId, contractId, request.Parts);

            return new ResponseData();
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="contractId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ResponseData> EditPaymentPartPercents(int userId, int contractId, PlatformContractChangePaymentPartPercentsDto request)
        {
            if (!await _platformContractDbService.IsCustomer(contractId, userId))
                return ResponseData.Fail("User", "Access denied!");

            PlatformContract contract = await _platformContractDbService.Get(contractId);

            if (contract.IsPrepayment != request.IsPrepayment)
            {
                contract.IsPrepayment = request.IsPrepayment;
                await _platformContractDbService.Update(contract);
            }

            foreach (PlatformContractPaymentPartDto part in request.Percents)
                part.Value *= contract.Price.HasValue ? contract.Price.Value : 0;

            await UpdateParts(userId, contractId, request.Percents);

            return new ResponseData();
        }

        public async Task<Dictionary<DateTime, List<PlatformContractChangeDto>>> List(int userId, int contractId)
        {
            if (!await _platformContractDbService.IsCustomerOrExecutor(contractId, userId))
                return null;

            return (await _platformContractChangeDbService.List(contractId))
                .GroupBy(c => c.DateTime)
                .ToDictionary(c => c.Key, c => c.Select(c => new PlatformContractChangeDto
                {
                    UserId = c.ApplicationUserId,
                    UserName = c.ApplicationUser.Name,
                    From = c.From,
                    To = c.To,
                    Case = c.Case
                }).ToList());
        }

        public async Task<ResponseData<PlatformContractApprovalDto>> GetApproval(int userId, int contractId)
        {
            if (!await _platformContractDbService.IsCustomerOrExecutor(contractId, userId))
                return ResponseData<PlatformContractApprovalDto>.Fail("User", "You not customer or executor of this contract!");

            PlatformContract contract = await _platformContractDbService.GetApproval(contractId);
            PlatformEstimate estimate = await _platformEstimateDbService.GetActual(contractId);

            return new ResponseData<PlatformContractApprovalDto>
            {
                Data = new PlatformContractApprovalDto
                {
                    ContractName = contract.Name,
                    BuildStart = contract.BuildStart,
                    PlannedBuildEnd = contract.PlannedBuildEnd,
                    Description = contract.Description,
                    GeneralTerm = contract.GeneralTerm,
                    Price = contract.Price,
                    EstimateName = estimate.Name,
                    EstimateFileName = estimate.FileName,

                    CooperationType = contract.CooperationType,
                    ComissionType = contract.ComissionType,
                    PaymentType = contract.PaymentType,
                    BudgetOfWorks = contract.BudgetOfWorks,
                    HoursOfWorks = contract.HoursOfWorks,
                    CostOfHour = contract.CostOfHour,
                    PaymentFrequency = contract.PaymentFrequency,

                    IsPrepayment = contract.IsPrepayment,
                    PrepaymentPercent = contract.PrepaymentPercent,
                    PrepaymentValue = contract.PrepaymentValue,

                    PaymentParts = contract.Parts.Select(p => new PlatformContractPaymentPartDto
                    {
                        Id = p.Id,
                        DateTime = p.DateTime,
                        Name = p.Name,
                        Number = p.Number,
                        Value = p.Value,
                        Percent = p.Percent,
                        PercentOfWork = p.PercentOfWork
                    }).ToList()
                }
            };
        }
    }
}
