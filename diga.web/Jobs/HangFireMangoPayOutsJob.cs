using diga.bl.Constants;
using diga.bl.Models.Platform.Contracts;
using diga.dal.DbServices.PlatformContractDbServices;
using diga.dal.DbServices.PlatformContractPaymentPartDbServices;
using diga.dal.DbServices.PlatformMeasurementReportDbServices;
using diga.dal.DbServices.PlatformPurchaseDbServises;
using diga.dal.DbServices.UserDbServices;
using diga.web.Helpers;
using diga.web.Services.MangoPayServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Jobs
{
    public interface IHangFireMangoPayOutsJob
    {
        Task Check();
    }
    public class HangFireMangoPayOutsJob : IHangFireMangoPayOutsJob
    {
        private readonly IPlatformPurchaseDbService _purchaseDbService;
        private readonly IUserDbService _userDbService;
        private readonly IMangoPayPayOutService _mangoPayPayOutService;

        private readonly IPlatformContractDbService _platformContractDbService;
        private readonly IPlatformMeasurementReportDbService _platformMeasurementReportDbService;
        private readonly IPlatformContractPaymentPartDbService _platformContractPaymentPartDbService;

        private readonly EmailHelper _emailHelper;

        public HangFireMangoPayOutsJob(IUserDbService userDbService,

            IPlatformContractDbService platformContractDbService,
            IPlatformMeasurementReportDbService platformMeasurementReportDbService,
            IPlatformContractPaymentPartDbService platformContractPaymentPartDbService,

            IPlatformPurchaseDbService purchaseDbService, 
            IMangoPayPayOutService mangoPayPayOutService, 
            EmailHelper emailHelper)
        {
            _purchaseDbService = purchaseDbService;

            _platformContractDbService = platformContractDbService;
            _platformMeasurementReportDbService = platformMeasurementReportDbService;
            _platformContractPaymentPartDbService = platformContractPaymentPartDbService;

            _userDbService = userDbService;
            _mangoPayPayOutService = mangoPayPayOutService;
            _emailHelper = emailHelper;
        }

        public async Task Check()
        {
            var payOuts = await _purchaseDbService.ListMangoPayOuts();
            var administrators = await _userDbService.ListByRole("Admin");

            foreach (var payOut in payOuts)
            {
                var user = await _userDbService.GetFull(payOut.UserId);
                if (user == null) continue;
                if (user.MangoPayKYCValidationStatus != MangoPayKYCValidationStatuses.VALIDATED) continue;
                if (user.PlatformMangoPayUserBankAccounts.Count == 0) continue;

                var result = await _mangoPayPayOutService.ToBankAccount(user.Id, (long)payOut.Price, payOut.ExternalIdentificator);
                if (result == null) continue;
                if (result.Status == MangoPay.SDK.Core.Enumerations.TransactionStatus.SUCCEEDED)
                {
                    payOut.Status = bl.Enums.PlatformPurchases.PaymentStatus.Successful;
                    payOut.ActualDate = DateTime.Now;
                    payOut.ExternalIdentificator = result.Id;

                    PlatformContract contract;
                    switch (payOut.EntityType)
                    {
                        case PlatformPayInTypes.PaymentPart:
                            var paymentPart = await _platformContractPaymentPartDbService.Get(payOut.EntityId.Value);
                            paymentPart.IsPaid = true;
                            await _platformContractPaymentPartDbService.Update(paymentPart);
                            break;
                        case PlatformPayInTypes.Prepayment:
                            contract = await _platformContractDbService.GetFull(payOut.EntityId.Value);
                            contract.IsPrepaymentPaid = true;
                            await _platformContractDbService.Update(contract);
                            break;
                        case PlatformPayInTypes.MeasurementReport:
                            var report = await _platformMeasurementReportDbService.Get(payOut.EntityId.Value);
                            report.IsPaid = true;
                            await _platformMeasurementReportDbService.Update(report);
                            break;
                        case PlatformPayInTypes.Reserve:
                            contract = await _platformContractDbService.GetFull(payOut.EntityId.Value);
                            contract.IsReservationPaid = true;
                            await _platformContractDbService.Update(contract);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    payOut.Status = bl.Enums.PlatformPurchases.PaymentStatus.Failed;
                    if (administrators.Count > 0)
                        await _emailHelper.SendAsync(administrators.First().Email, $"Payout to bank account failed for {user.Email} at {DateTime.Now.ToString("dd.MM.yyyy hh:mm")}",
                            $"<b>User</b>: {user.Email} <br> <b>Bank account</b>: {result.BankAccountId} <br> <b>Result mesage</b>: {result.ResultMessage}");
                }

            }

            await _purchaseDbService.UpdateRange(payOuts);
        }
    }
}
