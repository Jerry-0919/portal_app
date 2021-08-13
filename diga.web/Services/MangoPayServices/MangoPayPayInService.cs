using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diga.dal.DbServices.PlatformMangoPayUserBankAccountDbServices;
using diga.dal.DbServices.PlatformMangoPayUserKYCDocumentDbService;
using diga.dal.DbServices.UserDbServices;
using diga.web.Models.Pagination;
using diga.web.Models.PlatformPayServices;
using diga.web.Models.Status;
using diga.web.Options;
using MangoPay.SDK;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Entities.GET;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace diga.web.Services.MangoPayServices
{
    public class MangoPayPayInService : IMangoPayPayInService
    {
        private readonly IUserDbService _userDbService;
        private readonly MangoPayOptions _mangoPayOptions;
        private readonly MangoPayApi _api;

        public MangoPayPayInService(IOptions<MangoPayOptions> mangoPayOptions,
            IUserDbService userDbService)
        {
            _userDbService = userDbService;
            _mangoPayOptions = mangoPayOptions.Value;

            _api = new MangoPayApi();
            _api.Config.ClientId = _mangoPayOptions.ClientId;
            _api.Config.ClientPassword = _mangoPayOptions.ClientPassword;
            _api.Config.BaseUrl = _mangoPayOptions.BaseUrl;
        }


        public async Task<PayInBankWireDirectDTO> CreateBankWire(int userId, long amount, long fee)
        {
            var applicationUser = await _userDbService.GetFull(userId);
            if (string.IsNullOrEmpty(applicationUser.MangoPayUserId) || string.IsNullOrEmpty(applicationUser.MangoPayWalletId))
            {
                throw new System.Exception("Payment user id or wallet id not registered");
            }

            return await _api.PayIns.CreateBankWireDirectAsync(new MangoPay.SDK.Entities.POST.PayInBankWireDirectPostDTO(
                applicationUser.MangoPayUserId,
                applicationUser.MangoPayWalletId,
                new MangoPay.SDK.Entities.Money { Amount = amount, Currency = CurrencyIso.EUR },
                new MangoPay.SDK.Entities.Money { Amount = fee, Currency = CurrencyIso.EUR }));
        }

        public async Task<PayInCardDirectDTO> PayWithCard(int userId, string cardId, long amount, long fee, string returnUrl)
        {
            var applicationUser = await _userDbService.GetFull(userId);
            if (string.IsNullOrEmpty(applicationUser.MangoPayUserId) || string.IsNullOrEmpty(applicationUser.MangoPayWalletId))
            {
                throw new System.Exception("Payment user id or wallet id not registered");
            }

            var payIn = new MangoPay.SDK.Entities.POST.PayInCardDirectPostDTO(
                applicationUser.MangoPayUserId,
                applicationUser.MangoPayUserId,
                new MangoPay.SDK.Entities.Money { Amount = amount, Currency = CurrencyIso.EUR },
                new MangoPay.SDK.Entities.Money { Amount = fee, Currency = CurrencyIso.EUR },
                applicationUser.MangoPayWalletId,
                returnUrl,
                cardId);
            payIn.SecureMode = SecureMode.FORCE;

            return await _api.PayIns.CreateCardDirectAsync(payIn);
        }
    }
}