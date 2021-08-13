using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using diga.bl.Models; 
using diga.dal;
using Microsoft.AspNetCore.Authorization;

namespace diga.web.Controllers.Administration
{
    [Authorize(Roles = "Admin")]
    public class AdminModuleSectionsController : Controller
    {
        private readonly DigaContext _context;

        public AdminModuleSectionsController(DigaContext context)
        {
            _context = context;
        }

        // GET: AdminModuleSections
        public async Task<IActionResult> Index()
        {
            var digaContext = _context.ModuleSections.Include(m => m.ButtonText).Include(m => m.Module).Include(m => m.Text);
            return View(await digaContext.ToListAsync());
        }

        // GET: AdminModuleSections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moduleSections = await _context.ModuleSections
                .Include(m => m.ButtonText)
                .Include(m => m.Module)
                .Include(m => m.Text)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moduleSections == null)
            {
                return NotFound();
            }

            return View(moduleSections);
        }

        // GET: AdminModuleSections/Create
        public IActionResult Create()
        {
            ViewData["ButtonTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "BigTitleTextId");
            ViewData["TextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            return View();
        }

        // POST: AdminModuleSections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModuleId,ButtonTextId,TextId")] ModuleSections moduleSections)
        {
            if (ModelState.IsValid)
            {
                _context.Add(moduleSections);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ButtonTextId"] = new SelectList(_context.Texts, "TextId", "TextId", moduleSections.ButtonTextId);
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "BigTitleTextId", moduleSections.ModuleId);
            ViewData["TextId"] = new SelectList(_context.Texts, "TextId", "TextId", moduleSections.TextId);
            return View(moduleSections);
        }

        // GET: AdminModuleSections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moduleSections = await _context.ModuleSections.FindAsync(id);
            if (moduleSections == null)
            {
                return NotFound();
            }
            ViewData["ButtonTextId"] = new SelectList(_context.Texts, "TextId", "TextId", moduleSections.ButtonTextId);
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "BigTitleTextId", moduleSections.ModuleId);
            ViewData["TextId"] = new SelectList(_context.Texts, "TextId", "TextId", moduleSections.TextId);
            return View(moduleSections);
        }

        // POST: AdminModuleSections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModuleId,ButtonTextId,TextId")] ModuleSections moduleSections)
        {
            if (id != moduleSections.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(moduleSections);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModuleSectionsExists(moduleSections.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ButtonTextId"] = new SelectList(_context.Texts, "TextId", "TextId", moduleSections.ButtonTextId);
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "BigTitleTextId", moduleSections.ModuleId);
            ViewData["TextId"] = new SelectList(_context.Texts, "TextId", "TextId", moduleSections.TextId);
            return View(moduleSections);
        }

        // GET: AdminModuleSections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moduleSections = await _context.ModuleSections
                .Include(m => m.ButtonText)
                .Include(m => m.Module)
                .Include(m => m.Text)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (moduleSections == null)
            {
                return NotFound();
            }

            return View(moduleSections);
        }

        // POST: AdminModuleSections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var moduleSections = await _context.ModuleSections.FindAsync(id);
            _context.ModuleSections.Remove(moduleSections);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModuleSectionsExists(int id)
        {
            return _context.ModuleSections.Any(e => e.Id == id);
        }
    }
}
