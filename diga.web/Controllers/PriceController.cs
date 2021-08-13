using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diga.bl.Models; using diga.dal;
using diga.web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace diga.web.Controllers
{
    public class PriceController : Controller
    {
        private readonly DigaContext _context;

        public PriceController(DigaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Cart()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cart(CartModel model)
        {
            var totalPrice = 0.0;

            ViewBag.numberOfUsers = model.NumberOfUsers;

            var priceForUsers = 0.0;
            if (model.NumberOfUsers >10){
                var pricePerUser = 3.0;
                if (model.NumberOfUsers > 20){
                    pricePerUser = 2.5;
                }
                priceForUsers = (model.NumberOfUsers - 10) * pricePerUser * model.Term;
                totalPrice += priceForUsers;
            }
            ViewBag.priceForUsers = priceForUsers;

            var tariff = _context.Tariffs.Include(t => t.TariffModules).ThenInclude(tm => tm.Module).
                FirstOrDefault(t => t.Id == model.TariffId);
            if (tariff == null){
                return BadRequest();
            }
            totalPrice += tariff.Price * model.Term;
            ViewBag.tariff = tariff; 

            var priceForModules = 0.0;
            var modules = new List<Modules>();
            if (!string.IsNullOrEmpty(model.ModuleIds))
            {
                var modulesStr = model.ModuleIds.Split('|');
                if (modulesStr.Count() > 0)
                {
                    modules = _context.Modules.Where(m => modulesStr.Contains(m.Id.ToString())).ToList();
                    priceForModules = modules.Sum(m => m.Price) * model.Term;
                    totalPrice += priceForModules;
                }
            }

            ViewBag.modules = modules;
            ViewBag.priceForModules = priceForModules;
                        
            var promocode = _context.Promocodes.FirstOrDefault(p => p.Value == model.Promocode);
            var discountForPromocode = 0.0;
            if (promocode != null){
                if (promocode.Discount > 0){
                    discountForPromocode = (totalPrice / 100) * promocode.Discount.Value;
                    totalPrice -= discountForPromocode;
                }
            }
            ViewBag.promocode = promocode;
            ViewBag.discountForPromocode = discountForPromocode;

            var discountForTerm = 0.0;
            if (model.Term == 12){
                discountForTerm = totalPrice / 100 * 15;
                totalPrice -= discountForTerm;
            }
            ViewBag.discountForTerm = discountForTerm;

            var iva = 0.0;
            if (model.Country == "Portugal"){
                iva = totalPrice * 0.23;
                totalPrice += iva;
            }
            ViewBag.iva = iva;

            ViewBag.totalPrice = totalPrice;

            var cart = new Carts{
                NumberOfUsers = model.NumberOfUsers,
                Term = model.Term,
                TariffId = model.TariffId,
                Modules = model.ModuleIds,
                TotalPrice = totalPrice,
                Country = model.Country
            };
            _context.Carts.Add(cart);
            _context.SaveChanges();
            ViewBag.cartId = cart.Id;
            ViewBag.cart = cart;

            return View();
        }
    }
}