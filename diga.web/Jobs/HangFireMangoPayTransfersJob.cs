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
    public interface IHangFireMangoPayTransfersJob
    {
        Task Check();
    }

    public class HangFireMangoPayTransfersJob: IHangFireMangoPayTransfersJob
    {
        private readonly IPlatformPurchaseDbService _purchaseDbService;
        private readonly IUserDbService _userDbService;
        private readonly IMangoPayTransferService _mangoPayTransferService;

        private readonly IPlatformContractDbService _platformContractDbService;
        private readonly IPlatformMeasurementReportDbService _platformMeasurementReportDbService;
        private readonly IPlatformContractPaymentPartDbService _platformContractPaymentPartDbService;

        private readonly EmailHelper _emailHelper;
        private readonly MangoPayHelper _mangoPayHelper;

        public HangFireMangoPayTransfersJob(IUserDbService userDbService,

            IPlatformContractDbService platformContractDbService,
            IPlatformMeasurementReportDbService platformMeasurementReportDbService,
            IPlatformContractPaymentPartDbService platformContractPaymentPartDbService,

            IPlatformPurchaseDbService purchaseDbService,
            IMangoPayTransferService mangoPayTransferService,
            EmailHelper emailHelper,
            MangoPayHelper mangoPayHelper)
        {
            _purchaseDbService = purchaseDbService;

            _platformContractDbService = platformContractDbService;
            _platformMeasurementReportDbService = platformMeasurementReportDbService;
            _platformContractPaymentPartDbService = platformContractPaymentPartDbService;

            _userDbService = userDbService;
            _mangoPayTransferService = mangoPayTransferService;
            _emailHelper = emailHelper;
            _mangoPayHelper = mangoPayHelper;
        }

        public async Task Check()
        {
            var transfers = await _purchaseDbService.ListMangoPayTransfers();
            var administrators = await _userDbService.ListByRole("Admin");

            foreach (var transfer in transfers)
            {
                //if the depending payOut is not still succeded then skip
                if (transfer.ParentPlatformPurchase.Status != bl.Enums.PlatformPurchases.PaymentStatus.Successful)
                    continue;

                var recipient = await _userDbService.GetFull(transfer.UserId);
                var sender = await _userDbService.GetFull(transfer.ParentPlatformPurchase.UserId);

                if (recipient == null || sender == null) continue;
                if (string.IsNullOrEmpty(recipient.MangoPayWalletId) || string.IsNullOrEmpty(sender.MangoPayWalletId)) continue;

                var result = await _mangoPayTransferService.Make(sender.Id, recipient.Id, (long) transfer.Price);

                if (result == null) continue;
                if (result.Status == MangoPay.SDK.Core.Enumerations.TransactionStatus.SUCCEEDED)
                {
                    transfer.Status = bl.Enums.PlatformPurchases.PaymentStatus.Successful;
                    transfer.ActualDate = DateTime.Now;
                    transfer.ExternalIdentificator = result.Id;

                    await _purchaseDbService.Add(new bl.Models.Platform.PlatformPurchases.PlatformPurchase { 
                        UserId = transfer.UserId,
                        PaymentProvider = bl.Enums.PlatformPurchases.PaymentProvider.MangoPayOut,
                        Date = DateTime.Now,
                        Price = transfer.Price,
                        EntityId = transfer.EntityId,
                        EntityType = transfer.EntityType,
                        PaymentMethod = transfer.PaymentMethod,
                        Status = bl.Enums.PlatformPurchases.PaymentStatus.Created,
                        ParentId = transfer.Id
                    });
                }
                else
                {
                    transfer.Status = bl.Enums.PlatformPurchases.PaymentStatus.Failed;
                    if (administrators.Count > 0)
                        await _emailHelper.SendAsync(administrators.First().Email, $"MangoPay wallet transfer failed from {sender.Email} to {recipient.Email} at {DateTime.Now.ToString("dd.MM.yyyy hh:mm")}",
                            $"<b>Users</b>: {sender.Email} => {recipient.Email} <br> <b>Wallets</b>: {sender.MangoPayWalletId} => {recipient.MangoPayWalletId} <br><b>Amount</b>:{(long) transfer.Price} <br> <b>Result mesage</b>: {result.ResultMessage}");
                }

            }

            await _purchaseDbService.UpdateRange(transfers);
        }
    }
}
