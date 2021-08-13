using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diga.bl.Models; using diga.dal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace diga.web.Controllers
{
    public class FunctionsController : Controller
    {
        private readonly DigaContext _context;

        public FunctionsController(DigaContext context)
        {
            _context = context;
        }
        public IActionResult CompanyCard()
        {
            var function = _context.Functions.FirstOrDefault(m => m.Id == 2);
            return View(function);
        }
        public IActionResult Tasks()
        {
            var function = _context.Functions.FirstOrDefault(m => m.Id == 4);
            return View(function);
        }
        public IActionResult Services()
        {
            var function = _context.Functions.FirstOrDefault(m => m.Id == 3);
            return View(function);
        }
        public IActionResult History()
        {
            var function = _context.Functions.FirstOrDefault(m => m.Id == 15);
            return View(function);
        }
    }
}