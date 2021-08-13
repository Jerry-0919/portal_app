using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diga.bl.Models; using diga.dal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace diga.web.Controllers
{
    public class ModulesController : Controller
    {
        private readonly DigaContext _context;

        public ModulesController(DigaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var digaContext = _context.Modules.Include(a => a.Functions).ThenInclude(f => f.SmallTitleText).Include(a => a.LongText).Include(a => a.BigTitleText).Include(a => a.SmallTitleText);
            return View(digaContext.ToList());
        }
        public IActionResult CRM()
        {
            var module = _context.Modules.FirstOrDefault(m => m.Id == 5);
            return View(module);
        }
        public IActionResult HR()
        {
            var module = _context.Modules.FirstOrDefault(m => m.Id == 6);
            return View(module);
        }
        public IActionResult Calendar()
        {
            var module = _context.Modules.FirstOrDefault(m => m.Id == 7);
            return View(module);
        }
        public IActionResult AccessSettings()
        {
            var module = _context.Modules.FirstOrDefault(m => m.Id == 10);
            return View(module);
        }
        public IActionResult Email()
        {
            var module = _context.Modules.FirstOrDefault(m => m.Id == 8);
            return View(module);
        }
        public IActionResult Estimates()
        {
            var module = _context.Modules.FirstOrDefault(m => m.Id == 2);
            return View(module);
        }
        public IActionResult Dashboard()
        {
            var module = _context.Modules.FirstOrDefault(m => m.Id == 3);
            return View(module);
        }
        public IActionResult ProjectPlanner()
        {
            var module = _context.Modules.FirstOrDefault(m => m.Id == 1);
            return View(module);
        }
        public IActionResult GanttChart()
        {
            var module = _context.Modules.FirstOrDefault(m => m.Id == 10);
            return View(module);
        }
    }
}