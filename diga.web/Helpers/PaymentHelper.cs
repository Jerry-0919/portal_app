using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text;
using diga.web.ViewModels;
using Microsoft.EntityFrameworkCore;
using diga.bl.Models;
using diga.dal;

namespace diga.web.Helpers
{
    public interface IPaymentHelper
    {
        Task SuccessfullPayment(Carts cart);
    }
    public class PaymentHelper: IPaymentHelper
    {
        IConfiguration _config;
        private readonly DigaContext _context;
        private readonly SaasHelper _saasHelper;

        public PaymentHelper(IConfiguration config, DigaContext context, SaasHelper saasHelper)
        {
            _config = config;
            _context = context;
            _saasHelper = saasHelper;
        }

        public async Task SuccessfullPayment(Carts cart)
        {
            var userTarriff = await _context.UserTariffs.
                Include(ut => ut.Tariff).
                ThenInclude(t => t.TariffModules).
                ThenInclude(tm => tm.Module).
                FirstOrDefaultAsync(ut => ut.UserId == cart.UserId);

            var modules = userTarriff?.Tariff.TariffModules.Select(tm => new
            {
                id = tm.Module.Id,
                name = tm.Module.Name,
                current_subscription_date_start = userTarriff.Start.ToString("yyyy-MM-dd"),
                current_subscription_date_end = userTarriff.End.ToString("yyyy-MM-dd"),
                trial_date_start = (DateTime?)null,
                trial_date_end = (DateTime?)null,
            }).ToList();

            var boughtModuleIds = cart.Modules.Split('|');
            var actualModules = new List<Modules>();
            if (boughtModuleIds.Count() > 0)
            {
                actualModules = _context.Modules.Where(m => boughtModuleIds.Contains(m.Id.ToString())).ToList();

                if (modules == null)
                {
                    modules = actualModules.Select(m => new {
                        id = m.Id,
                        name = m.Name,
                        current_subscription_date_start = DateTime.Now.ToString("yyyy-MM-dd"),
                        current_subscription_date_end = DateTime.Now.AddMonths(cart.Term).ToString("yyyy-MM-dd"),
                        trial_date_start = (DateTime?)null,
                        trial_date_end = (DateTime?)null
                    }).ToList();
                }
                else
                {
                    modules.AddRange(actualModules.Select(m => new {
                        id = m.Id,
                        name = m.Name,
                        current_subscription_date_start = DateTime.Now.ToString("yyyy-MM-dd"),
                        current_subscription_date_end = DateTime.Now.AddMonths(cart.Term).ToString("yyyy-MM-dd"),
                        trial_date_start = (DateTime?)null,
                        trial_date_end = (DateTime?)null
                    }).ToList());
                }

            }

            var updateResult = await _saasHelper.ChangeModules(cart.User.AuthToken, JsonConvert.SerializeObject(modules), cart.NumberOfUsers);

            if (updateResult == false)
            {
                throw new Exception("Saas update modules exception");
            }

            foreach (var module in actualModules)
            {
                var userModule = await _context.UserModules.FirstOrDefaultAsync(um => um.UserId == cart.UserId && um.ModuleId == module.Id);
                if (userModule == null){
                    userModule = new UserModules{
                        ModuleId = module.Id,
                        UserId = cart.UserId.Value,
                        Start = DateTime.Now,
                        End = DateTime.Now.AddMonths(cart.Term),
                        IsTrial = false,
                        CurrentPrice = module.Price,
                    };
                    _context.UserModules.Add(userModule);
                }
                else{
                    userModule.Start = DateTime.Now;
                    userModule.End = DateTime.Now.AddMonths(cart.Term);
                    userModule.IsTrial = false;
                    userModule.CurrentPrice = module.Price;
                }
            }

            await _context.SaveChangesAsync();
        }              
    }
}
