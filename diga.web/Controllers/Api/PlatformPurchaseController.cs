using diga.bl.Enums.PlatformPurchases;
using diga.dal;
using diga.web.Helpers;
using diga.web.Models.PaidFeatures;
using diga.web.Models.PlatformPurchases;
using diga.web.Services.PlatformBalanceServices;
using diga.web.Services.PlatformPurchaseServices;
using diga.web.Services.ProductServices;
using diga.web.Services.UserServices;
using diga.web.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Controllers.Api
{
    [Route("api/purchase")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlatformPurchaseController : BaseController
    {
        private readonly IPlatformPurchaseService _purchaseService;
        private readonly IPlatformBalanceService _balanceService;
        private readonly IEupagoHelper _eupagoHelper;
        public PlatformPurchaseController(IPlatformPurchaseService purchaseService, IPlatformBalanceService balanceService, IEupagoHelper eupagoHelper)
            : base(null, null)
        {
            _purchaseService = purchaseService;
            _balanceService = balanceService;
            _eupagoHelper = eupagoHelper;
        }

        [HttpGet("{skip}/{take}")]
        public async Task<IActionResult> Get(int skip, int take)
        {
            if (User.Identity.AuthenticationType == "Admin")
            {
                return Ok(await _purchaseService.GetAll());
            }
            return Ok(await _purchaseService.GetPaginatedList(skip, take, UserId));
        }

        [HttpPost("multibanco/")]
        public async Task<IActionResult> PurchaseMultibanco(double amountPay,
            string cartId, PaidFeatureDto paidFeatureDto )
        {
            await _balanceService.Withdrawal(UserId, paidFeatureDto.Price - amountPay);
            await _eupagoHelper.Multibanco(amountPay, cartId);
            return Ok();
        }
        [HttpPost("mbway/")]
        public async Task<IActionResult> PurchaseMbway(double amountPay,
           string phone, string cartId, PaidFeatureDto paidFeatureDto)
        {
            await _balanceService.Withdrawal(UserId, paidFeatureDto.Price - amountPay);
            await _eupagoHelper.Mbway(amountPay,phone, cartId);
            return Ok();
        }
        [HttpPost("creditcard/")]
        public async Task<IActionResult> PurchaseCreditCard(double amountPay,
            string cartId,
            string cardNumber,
            string cvv,
            string validTo, PaidFeatureDto paidFeatureDto)
        {
            await _balanceService.Withdrawal(UserId, paidFeatureDto.Price - amountPay);
            await _eupagoHelper.CreditCard(amountPay,cartId,cardNumber,cvv,validTo);
            return Ok();
        }




    }
}
