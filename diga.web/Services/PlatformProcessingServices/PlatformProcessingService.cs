using diga.bl.Constants;
using diga.bl.Models.Platform.PlatformPurchases;
using diga.dal.DbServices.PlatformContractDbServices;
using diga.dal.DbServices.PlatformContractPaymentPartDbServices;
using diga.dal.DbServices.PlatformMeasurementReportDbServices;
using diga.dal.DbServices.PlatformPurchaseDbServises;
using diga.web.Helpers;
using diga.web.Models.PlatformPayIns;
using diga.web.Models.Status;
using diga.web.Options;
using diga.web.Services.MangoPayServices;
using MangoPay.SDK.Entities.GET;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.PlatformProcessingServices
{
    public class PlatformProcessingService : IPlatformProcessingService
    {
        private readonly IMangoPayPayInService _mangoPayPayInService;
        private readonly IPlatformContractDbService _platformContractDbService;
        private readonly IPlatformMeasurementReportDbService _platformMeasurementReportDbService;
        private readonly IPlatformContractPaymentPartDbService _platformContractPaymentPartDbService;
        private readonly IPlatformPurchaseDbService _purchaseDbService;
        private readonly DomainOptions _domainOptions;
        private readonly MangoPayHelper _mangoPayHelper;

        public PlatformProcessingService(IMangoPayPayInService mangoPayPayInService,
            IPlatformContractDbService platformContractDbService,
            IPlatformMeasurementReportDbService platformMeasurementReportDbService,
            IPlatformContractPaymentPartDbService platformContractPaymentPartDbService,
            IPlatformPurchaseDbService purchaseDbService,
            IOptions<DomainOptions> domainOptions,
            MangoPayHelper mangoPayHelper)
        {
            _mangoPayPayInService = mangoPayPayInService;
            _platformContractDbService = platformContractDbService;
            _platformMeasurementReportDbService = platformMeasurementReportDbService;
            _platformContractPaymentPartDbService = platformContractPaymentPartDbService;
            _purchaseDbService = purchaseDbService;
            _domainOptions = domainOptions.Value;
            _mangoPayHelper = mangoPayHelper;
        }

        public async Task<ResponseData> ConfirmPayIn(int userId, PlatformPayInConfirmDto request)
        {
            var contract = await _platformContractDbService.GetFull(request.ContractId);
            if (contract == null)
            {
                return ResponseData.Fail("Contract", "Contract not found");
            }

            if (!(await _platformContractDbService.IsCustomer(request.ContractId, userId)))
            {
                return ResponseData.Fail("User", "You are not customer of the contract");
            }

            switch (request.Type)
            {
                case PlatformPayInTypes.PaymentPart:
                    var paymentPart = await _platformContractPaymentPartDbService.Get(request.Id);
                    paymentPart.IsMade = true;
                    await _platformContractPaymentPartDbService.Update(paymentPart);
                    //TODO call the payOut
                    break;
                case PlatformPayInTypes.Prepayment:
                    contract.IsPrepaymentMade = true;
                    await _platformContractDbService.Update(contract);
                    break;
                case PlatformPayInTypes.MeasurementReport:
                    var report = await _platformMeasurementReportDbService.Get(request.Id);
                    report.IsMade = true;
                    await _platformMeasurementReportDbService.Update(report);
                    //TODO call the payOut
                    break;
                case PlatformPayInTypes.Reserve:
                    contract.IsResevationMade = true;
                    await _platformContractDbService.Update(contract);
                    break;
                default:
                    break;
            }

            return ResponseData.Ok();
        }

        public async Task<PlatformPayInDto> ProcessPayIn(int userId, PlatformPayInAddDto request)
        {
            var contract = await _platformContractDbService.GetFull(request.ContractId);
            if (contract == null)
            {
                throw new Exception("Contract not found");
            }

            if (!(await _platformContractDbService.IsCustomer(request.ContractId, userId)))
            {
                throw new Exception("You are not customer of the contract");
            }
            var executor = await _platformContractDbService.GetExecutor(request.ContractId);

            PlatformPayInDto result = null;
            PayInBankWireDirectDTO payInBank = null;
            PayInCardDirectDTO payInCard = null;
            PlatformPurchase purchase = new PlatformPurchase {
                UserId = userId,
                Date = DateTime.Now,
                Status = bl.Enums.PlatformPurchases.PaymentStatus.Created,
                PaymentProvider = bl.Enums.PlatformPurchases.PaymentProvider.MangoPayIn,
                PaymentMethod = request.Method,
                DebitedFromBalance = 0,
                EntityType = request.Type
            };

            switch (request.Type)
            {
                case PlatformPayInTypes.PaymentPart:
                    purchase.EntityId = request.PaymentPartId;
                    break;
                case PlatformPayInTypes.Prepayment:
                    purchase.EntityId = request.ContractId;
                    if (contract.IsPrepayment != true || contract.PrepaymentValue == null || contract.PrepaymentValue == 0)
                    {
                        throw new Exception("Contract is not prepayment or prepayment is not greater than zero");
                    }
                    purchase.Price = contract.PrepaymentValue.Value;
                    if (request.Method == PlatformPayInMethods.Card)
                    {
                        var fee = _mangoPayHelper.CalculateFee(contract.PrepaymentValue.Value, request.Method);
                        payInCard = await _mangoPayPayInService.PayWithCard(userId, request.CardId, (long)contract.PrepaymentValue, fee, $"{_domainOptions.Client}/platform/index#/payment?type=prepayment&contractId={contract.Id}");
                        if (payInCard.Status == MangoPay.SDK.Core.Enumerations.TransactionStatus.SUCCEEDED)
                        {
                            contract.IsPrepaymentMade = true;
                            await _platformContractDbService.Update(contract);
                            purchase.Status = bl.Enums.PlatformPurchases.PaymentStatus.Successful;
                            purchase.DebitedFromMoney = contract.PrepaymentValue.Value;
                        }
                    }
                    else
                    {
                        var fee = _mangoPayHelper.CalculateFee(contract.PrepaymentValue.Value, request.Method);
                        payInBank = await _mangoPayPayInService.CreateBankWire(userId, (long)contract.PrepaymentValue, fee);
                    }
                    break;
                case PlatformPayInTypes.MeasurementReport:
                    purchase.EntityId = request.MeasurementReportId;
                    if (request.MeasurementReportId == null || request.MeasurementReportId == 0)
                    {
                        throw new Exception("Measurement report id is not defined");
                    }
                    var report = await _platformMeasurementReportDbService.Get(request.MeasurementReportId.Value);
                    if (report.Status != PlatformMeasurementReportStatuses.Accepted || report.PriceWithVat == 0)
                    {
                        throw new Exception("Measurement report status is not Accepted or value is not greater than zero");
                    }
                    
                    purchase.Price = report.PriceWithVat;
                    if (request.Method == PlatformPayInMethods.Card)
                    {
                        var fee = _mangoPayHelper.CalculateFee(report.PriceWithVat, request.Method);
                        payInCard = await _mangoPayPayInService.PayWithCard(userId, request.CardId, (long)report.PriceWithVat, fee, $"{_domainOptions.Client}/platform/index#/payment?type=measurementReport&contractId={contract.Id}&id={report.Id}");
                        if (payInCard.Status == MangoPay.SDK.Core.Enumerations.TransactionStatus.SUCCEEDED)
                        {
                            report.IsMade = true;
                            await _platformMeasurementReportDbService.Update(report);
                            purchase.Status = bl.Enums.PlatformPurchases.PaymentStatus.Successful;
                            purchase.DebitedFromMoney = report.PriceWithVat;
                        }
                    }
                    else
                    {
                        var fee = _mangoPayHelper.CalculateFee(report.PriceWithVat, request.Method);
                        payInBank = await _mangoPayPayInService.CreateBankWire(userId, (long)report.PriceWithVat, fee);
                    }
                    break;
                case PlatformPayInTypes.Reserve:
                    purchase.EntityId = request.ContractId;
                    if (contract.Categories.Count() == 0 || !contract.Categories.Any(c => c.PlatformCategory.ReservationPercent > 0))
                    {
                        throw new Exception("Contract is not prepayment or prepayment is not greater than zero");
                    }
                    var reservationSum = (contract.Categories.Select(c => c.PlatformCategory.ReservationPercent).Max() / 100) * contract.Price;
                    if (reservationSum == null || reservationSum == 0)
                    {
                        throw new Exception("Reservation sum is not greater than zero");
                    }
                    purchase.Price = reservationSum.Value;
                    if (request.Method == PlatformPayInMethods.Card)
                    {
                        var fee = _mangoPayHelper.CalculateFee(reservationSum.Value, request.Method);
                        payInCard = await _mangoPayPayInService.PayWithCard(userId, request.CardId, (long)reservationSum, fee, $"{_domainOptions.Client}/platform/index#/payment?type=reservation&contractId={contract.Id}");
                        if (payInCard.Status == MangoPay.SDK.Core.Enumerations.TransactionStatus.SUCCEEDED)
                        {
                            contract.IsResevationMade = true;
                            await _platformContractDbService.Update(contract);
                            purchase.Status = bl.Enums.PlatformPurchases.PaymentStatus.Successful;
                            purchase.DebitedFromMoney = reservationSum.Value;
                        }
                    }
                    else
                    {
                        var fee = _mangoPayHelper.CalculateFee(reservationSum.Value, request.Method);
                        payInBank = await _mangoPayPayInService.CreateBankWire(userId, fee, 0);
                    }
                    break;
                default:
                    break;
            }
                       
            if (payInCard != null)
            {
                if (payInCard.Status == MangoPay.SDK.Core.Enumerations.TransactionStatus.FAILED)
                {
                    throw new Exception("Card payment failed");
                }
                purchase.ExternalIdentificator = payInCard.Id;
                result = new PlatformPayInDto
                {
                    RedirectURL = payInCard.SecureModeRedirectURL,
                    Status = payInCard.Status
                };
            }
            if (payInBank != null)
            {
                purchase.ExternalIdentificator = payInBank.Id;
                result = new PlatformPayInDto
                {
                    WireReference = payInBank.WireReference,
                    BankAccount = payInBank.BankAccount
                };
            }

            if (result == null)
            {
                throw new Exception("Card or bank is not defined");
            }

            await _purchaseDbService.Add(purchase);

            //transfer for executor (monitored by hangfire and if parent transaction succeded - processes it at HangFireMangoPayTransfersJob.cs)
            await _purchaseDbService.Add(new PlatformPurchase {
                UserId = executor.Id,
                Date = DateTime.Now,
                Status = bl.Enums.PlatformPurchases.PaymentStatus.Created,
                PaymentProvider = bl.Enums.PlatformPurchases.PaymentProvider.MangoPayTransfer,
                PaymentMethod = purchase.PaymentMethod,
                Price = _mangoPayHelper.CalculateTransfer(purchase.Price, purchase.EntityType),
                EntityType = purchase.EntityType,
                EntityId = purchase.EntityId,
                ParentId = purchase.Id
            });
            return result;
        }
    }
}
