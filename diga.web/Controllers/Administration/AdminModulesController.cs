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
    public class AdminModulesController : Controller
    {
        private readonly DigaContext _context;

        public AdminModulesController(DigaContext context)
        {
            _context = context;
        }

        // GET: AdminModules
        public async Task<IActionResult> Index()
        {
            var digaContext = _context.Modules.Include(m => m.BigTitleText).Include(m => m.LongText).Include(m => m.SmallTitleText);
            return View(await digaContext.ToListAsync());
        }

        // GET: AdminModules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modules = await _context.Modules
                .Include(m => m.BigTitleText)
                .Include(m => m.LongText)
                .Include(m => m.SmallTitleText)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modules == null)
            {
                return NotFound();
            }

            return View(modules);
        }

        // GET: AdminModules/Create
        public IActionResult Create()
        {
            ViewData["BigTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["SmallTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            return View();
        }

        // POST: AdminModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BigTitleTextId,SmallTitleTextId,PictureUrl,LongTextId,Price,Enabled,Order")] Modules modules)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modules);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BigTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", modules.BigTitleTextId);
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId", modules.LongTextId);
            ViewData["SmallTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", modules.SmallTitleTextId);
            return View(modules);
        }

        // GET: AdminModules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modules = await _context.Modules.FindAsync(id);
            if (modules == null)
            {
                return NotFound();
            }
            ViewData["BigTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", modules.BigTitleTextId);
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId", modules.LongTextId);
            ViewData["SmallTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", modules.SmallTitleTextId);
            return View(modules);
        }

        // POST: AdminModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BigTitleTextId,SmallTitleTextId,PictureUrl,LongTextId,Price,Enabled,Order")] Modules modules)
        {
            if (id != modules.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modules);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModulesExists(modules.Id))
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
            ViewData["BigTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", modules.BigTitleTextId);
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId", modules.LongTextId);
            ViewData["SmallTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", modules.SmallTitleTextId);
            return View(modules);
        }

        // GET: AdminModules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modules = await _context.Modules
                .Include(m => m.BigTitleText)
                .Include(m => m.LongText)
                .Include(m => m.SmallTitleText)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modules == null)
            {
                return NotFound();
            }

            return View(modules);
        }

        // POST: AdminModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modules = await _context.Modules.FindAsync(id);
            _context.Modules.Remove(modules);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModulesExists(int id)
        {
            return _context.Modules.Any(e => e.Id == id);
        }
    }
}
