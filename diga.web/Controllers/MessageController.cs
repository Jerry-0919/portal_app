using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using diga.bl.Models; using diga.dal;
using diga.web.Auth;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using diga.web.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using diga.web.Helpers;
using Microsoft.Extensions.Configuration;

namespace diga.web.Controllers
{
    public class MessageController:Controller
    {
        private DigaContext db;
        IPaymentHelper paymentHelper;
        public MessageController(DigaContext context, IPaymentHelper paymentHelper)
        {
            db = context;
            this.paymentHelper = paymentHelper;
        }
        public IActionResult FormSubmitSuccess()
        {
            return View();
        }

        public IActionResult TrialSuccess()
        {
            ViewBag.isError = bool.Parse(HttpContext.Session.GetString("TrialError"));
            ViewBag.errorMessage = HttpContext.Session.GetString("TrialErrorMessage");
            return View();
        }

        [HttpGet("SuccessCreditCardPayment/{cartId}")]
        public async Task<IActionResult> SuccessCreditCardPayment(int cartId)
        {
            var cart = await db.Carts.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart == null){
                return BadRequest();
            }
            await paymentHelper.SuccessfullPayment(cart);
                        
            return View();
        }
    }
}
