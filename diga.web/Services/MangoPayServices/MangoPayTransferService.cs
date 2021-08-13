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
    public class MangoPayTransferService : IMangoPayTransferService
    {
        private readonly IUserDbService _userDbService;
        private readonly MangoPayOptions _mangoPayOptions;
        private readonly MangoPayApi _api;

        public MangoPayTransferService(IOptions<MangoPayOptions> mangoPayOptions,
            IUserDbService userDbService)
        {
            _userDbService = userDbService;
            _mangoPayOptions = mangoPayOptions.Value;

            _api = new MangoPayApi();
            _api.Config.ClientId = _mangoPayOptions.ClientId;
            _api.Config.ClientPassword = _mangoPayOptions.ClientPassword;
            _api.Config.BaseUrl = _mangoPayOptions.BaseUrl;
        }

        public async Task<TransferDTO> Make(int creditedUserId, int debitedUserId, long amount)
        {
            var creditedUser = await _userDbService.Get(creditedUserId);
            var debitedUser = await _userDbService.Get(debitedUserId);

            if (string.IsNullOrEmpty(creditedUser.MangoPayUserId) || 
                string.IsNullOrEmpty(creditedUser.MangoPayWalletId) ||
                string.IsNullOrEmpty(debitedUser.MangoPayUserId) ||
                string.IsNullOrEmpty(debitedUser.MangoPayWalletId) )
            {
                return null;
            }

            return await _api.Transfers.CreateAsync(new MangoPay.SDK.Entities.POST.TransferPostDTO(creditedUser.MangoPayUserId,
                creditedUser.MangoPayUserId,
                new MangoPay.SDK.Entities.Money { Amount = amount, Currency = MangoPay.SDK.Core.Enumerations.CurrencyIso.EUR},
                new MangoPay.SDK.Entities.Money { Amount = 0, Currency = MangoPay.SDK.Core.Enumerations.CurrencyIso.EUR },
                debitedUser.MangoPayWalletId,
                creditedUser.MangoPayWalletId
            ));
        }
    }
}
