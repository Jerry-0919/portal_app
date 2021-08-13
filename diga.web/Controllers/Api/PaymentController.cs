using diga.dal;
using diga.web.Controllers.Api;
using diga.web.Helpers;
using diga.web.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace diga.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseController
    {
        public PaymentController(DigaContext _db, IConfiguration config, IPaymentHelper paymentHelper, IEupagoHelper eupagoHelper, SaasHelper saasHelper) : base(_db, config, paymentHelper, eupagoHelper, saasHelper)
        {
        }

        [AllowAnonymous]
        [HttpGet("/api/eupago_confirm")]
        public async Task<IActionResult> ConfirmEupago(double value, string channel, string reference, string transaction, int id)
        {
            var cart = await _db.Carts.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
            if (cart == null)
            {
                return BadRequest();
            }
            await _paymentHelper.SuccessfullPayment(cart);

            cart.IsPaid = true;
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("makeReturn")]
        [Authorize(AuthenticationSchemes =
            JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> MakeReturn()
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            if (user == null)
            {
                return BadRequest();
            }
            var toReturn = 0.0;

            var userTarriff = await _db.UserTariffs.
            Include(ut => ut.Tariff).
            ThenInclude(t => t.TariffModules).
            ThenInclude(tm => tm.Module).
            OrderByDescending(ut => ut.End).
            FirstOrDefaultAsync(ut =>
                ut.CurrentPrice > 0
                && ut.UserId == user.Id
                && ut.End > DateTime.Now
            );
            if (userTarriff != null)
            {
                var tDays = (userTarriff.End - userTarriff.Start).TotalDays;
                var tPricePerDay = userTarriff.CurrentPrice / tDays;
                var tDaysLeft = (userTarriff.End - DateTime.Now).TotalDays;
                var tPriceLeft = tDaysLeft * tPricePerDay;
                if (tPriceLeft != null)
                {
                    toReturn += tPriceLeft.Value;
                }
                userTarriff.End = DateTime.Now;
            }

            var userModules = await _db.UserModules.Include(um => um.Module).OrderByDescending(ut => ut.End).Where(ut =>
                ut.CurrentPrice > 0
                && ut.UserId == user.Id
                && ut.End > DateTime.Now
            ).ToListAsync();
            if (userModules.Count > 0)
            {
                foreach (var um in userModules)
                {
                    var mDays = (um.End - um.Start).TotalDays;
                    var mPricePerDay = um.CurrentPrice / mDays;
                    var mDaysLeft = (um.End - DateTime.Now).TotalDays;
                    var mPriceLeft = mDaysLeft * mPricePerDay;
                    if (mPriceLeft != null)
                    {
                        toReturn += mPriceLeft.Value;
                    }
                    um.End = DateTime.Now;
                }
            }

            if (toReturn > 0)
            {
                if (user.Balance == null)
                {
                    user.Balance = toReturn;
                }
                else
                {
                    user.Balance += toReturn;
                }
            }

            var modules = userTarriff.Tariff.TariffModules.Select(tm => new
            {
                name = tm.Module.Name,
                current_subscription_date_start = userTarriff.Start.ToString("yyyy-MM-dd"),
                current_subscription_date_end = DateTime.Now.ToString("yyyy-MM-dd"),
                trial_date_start = (DateTime?)null,
                trial_date_end = (DateTime?)null
            }).ToList();
            modules.AddRange(userModules.Select(um => new {
                name = um.Module.Name,
                current_subscription_date_start = um.Start.ToString("yyyy-MM-dd"),
                current_subscription_date_end = DateTime.Now.ToString("yyyy-MM-dd"),
                trial_date_start = (DateTime?)null,
                trial_date_end = (DateTime?)null
            }).ToList());
            var updateResult = await _saasHelper.ChangeModules(user.AuthToken, JsonConvert.SerializeObject(modules), 0);

            if (updateResult == false)
            {
                return BadRequest();
            }

            await _db.SaveChangesAsync();

            return Ok(new
            {
                toReturn = toReturn,
                balance = user.Balance
            });
        }

        [HttpPost("confirmPayment")]
        [Authorize(AuthenticationSchemes =
            JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ConfirmPayment(ConfirmCartModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            if (user == null)
            {
                return BadRequest();
            }
            var cart = await _db.Carts.FirstOrDefaultAsync(c => c.Id == model.CartId);
            if (cart == null)
            {
                return BadRequest();
            }

            cart.UserId = user.Id;
            _db.SaveChanges();

            var paymentType = "money";
            var payMoney = cart.TotalPrice;
            var payBalance = 0.0;
            if (user.Balance > 0)
            {
                if (user.Balance >= cart.TotalPrice)
                {
                    paymentType = "balance";
                    payMoney = 0;
                    payBalance = cart.TotalPrice;

                    await _paymentHelper.SuccessfullPayment(cart);
                }
                else
                {
                    payMoney = cart.TotalPrice - user.Balance.Value;
                    payBalance = user.Balance.Value;
                }
                user.Balance = user.Balance - payBalance;
            }
            cart.TotalPriceWithDiscount = payMoney;
            cart.FromBalance = payBalance;
            await _db.SaveChangesAsync();

            return Ok(new
            {
                paymentType = paymentType,
                payMoney = payMoney,
                payBalance = payBalance
            });
        }

        [HttpPost("creditCard")]
        [Authorize(AuthenticationSchemes = 
            JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EupagoCreditCard(CreditCardModel model){
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            if (user == null){
                return BadRequest();
            }
            var cart = await _db.Carts.FirstOrDefaultAsync(c => c.Id == model.CartId);
            if (cart == null){
                return BadRequest();
            }

            if (cart.TotalPriceWithDiscount == null ||  cart.TotalPriceWithDiscount<= 0){
                return BadRequest();
            }

            var response = await _eupagoHelper.CreditCard(
                cart.TotalPriceWithDiscount.Value,
                cart.Id.ToString(),
                model.CardNumber,
                model.Cvv,
                model.ValidToMm + "/" + model.ValidToYy
            );
            cart.Reference = response.Reference;
            cart.Provider = "eupago_credit_card";
            
            await _db.SaveChangesAsync();

            return Ok(new{
                url = response.RedirectUrl
            });
        }

        [HttpPost("multibanco")]
        [Authorize(AuthenticationSchemes = 
            JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EupagoMultibanco(MultibancoModel model){
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            if (user == null){
                return BadRequest();
            }
            var cart = await _db.Carts.FirstOrDefaultAsync(c => c.Id == model.CartId);
            if (cart == null){
                return BadRequest();
            }

            if (cart.TotalPriceWithDiscount == null ||  cart.TotalPriceWithDiscount<= 0){
                return BadRequest();
            }

            var response = await _eupagoHelper.Multibanco(
                cart.TotalPriceWithDiscount.Value,
                cart.Id.ToString()
            );

            cart.Reference = response.Reference;
            cart.Provider = "eupago_multibanco";
            
            await _db.SaveChangesAsync();

            return Ok(new{
                reference = response.Reference,
                entity = response.Entity
            });
        }

        [HttpPost("mbway")]
        [Authorize(AuthenticationSchemes = 
            JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> EupagoMbway(MbwayModel model){
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            if (user == null){
                return BadRequest();
            }
            var cart = await _db.Carts.FirstOrDefaultAsync(c => c.Id == model.CartId);
            if (cart == null){
                return BadRequest();
            }

            if (cart.TotalPriceWithDiscount == null ||  cart.TotalPriceWithDiscount<= 0){
                return BadRequest();
            }

            var response = await _eupagoHelper.Mbway(
                cart.TotalPriceWithDiscount.Value,
                model.Phone,
                cart.Id.ToString()
            );

            cart.Reference = response.Reference;
            cart.Provider = "eupago_mbway";
            
            await _db.SaveChangesAsync();

            return Ok(new{
                reference = response.Reference,
                entity = response.Entity
            });
        }
    }
}