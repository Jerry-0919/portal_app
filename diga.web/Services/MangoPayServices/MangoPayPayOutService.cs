using diga.bl.Constants;
using diga.dal.DbServices.UserDbServices;
using diga.web.Options;
using MangoPay.SDK;
using MangoPay.SDK.Entities.GET;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Services.MangoPayServices
{
    public class MangoPayPayOutService : IMangoPayPayOutService
    {
        private readonly IUserDbService _userDbService;
        private readonly MangoPayOptions _mangoPayOptions;
        private readonly MangoPayApi _api;

        public MangoPayPayOutService(IOptions<MangoPayOptions> mangoPayOptions,
            IUserDbService userDbService)
        {
            _userDbService = userDbService;
            _mangoPayOptions = mangoPayOptions.Value;

            _api = new MangoPayApi();
            _api.Config.ClientId = _mangoPayOptions.ClientId;
            _api.Config.ClientPassword = _mangoPayOptions.ClientPassword;
            _api.Config.BaseUrl = _mangoPayOptions.BaseUrl;
        }

        public async Task<PayOutBankWireDTO> ToBankAccount(int userId, long amount, string bankRef)
        {
            var applicationUser = await _userDbService.GetFull(userId);
            if (string.IsNullOrEmpty(applicationUser.MangoPayUserId) || string.IsNullOrEmpty(applicationUser.MangoPayWalletId) || applicationUser.MangoPayKYCValidationStatus != MangoPayKYCValidationStatuses.VALIDATED)
            {
                return null;
            }

            var bankAccount = applicationUser.PlatformMangoPayUserBankAccounts.FirstOrDefault();
            if (bankAccount == null)
            {
                return null;
            }

            return await _api.PayOuts.CreateBankWireAsync(new MangoPay.SDK.Entities.POST.PayOutBankWirePostDTO(
                applicationUser.MangoPayUserId,
                applicationUser.MangoPayWalletId,
                new MangoPay.SDK.Entities.Money { Amount = amount, Currency = MangoPay.SDK.Core.Enumerations.CurrencyIso.EUR },
                new MangoPay.SDK.Entities.Money { Amount = 0, Currency = MangoPay.SDK.Core.Enumerations.CurrencyIso.EUR },
                bankAccount.MangoPayId,
                bankRef,
                "STANDARD"));
        }
    }
}
