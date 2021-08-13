using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using diga.bl.Models; using diga.dal;
using Microsoft.AspNetCore.Authorization;

namespace diga.web.Controllers.Administration
{
    [Authorize(Roles = "Admin")]
    public class AdminFunctionsController : Controller
    {
        private readonly DigaContext _context;

        public AdminFunctionsController(DigaContext context)
        {
            _context = context;
        }

        // GET: AdminFunctions
        public async Task<IActionResult> Index()
        {
            var digaContext = _context.Functions.Include(f => f.BigTitleText).Include(f => f.LongText).Include(f => f.Module).Include(f => f.SmallTitleText);
            return View(await digaContext.ToListAsync());
        }

        // GET: AdminFunctions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functions = await _context.Functions
                .Include(f => f.BigTitleText)
                .Include(f => f.LongText)
                .Include(f => f.Module)
                .Include(f => f.SmallTitleText)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (functions == null)
            {
                return NotFound();
            }

            return View(functions);
        }

        // GET: AdminFunctions/Create
        public IActionResult Create()
        {
            ViewData["BigTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "BigTitleTextId");
            ViewData["SmallTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["FullDescriptionTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            return View();
        }

        // POST: AdminFunctions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BigTitleTextId,SmallTitleTextId,PictureUrl,LongTextId,FullDescriptionTextId,ModuleId,Enabled")] Functions functions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(functions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BigTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", functions.BigTitleTextId);
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId", functions.LongTextId);
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "BigTitleTextId", functions.ModuleId);
            ViewData["SmallTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", functions.SmallTitleTextId);
            ViewData["FullDescriptionTextId"] = new SelectList(_context.Texts, "TextId", "TextId", functions.FullDescriptionTextId);
            return View(functions);
        }

        // GET: AdminFunctions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functions = await _context.Functions.FindAsync(id);
            if (functions == null)
            {
                return NotFound();
            }
            ViewData["BigTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", functions.BigTitleTextId);
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId", functions.LongTextId);
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "BigTitleTextId", functions.ModuleId);
            ViewData["SmallTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", functions.SmallTitleTextId);
            ViewData["FullDescriptionTextId"] = new SelectList(_context.Texts, "TextId", "TextId", functions.FullDescriptionTextId);
            return View(functions);
        }

        // POST: AdminFunctions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BigTitleTextId,SmallTitleTextId,PictureUrl,LongTextId,FullDescriptionTextId,ModuleId,Enabled")] Functions functions)
        {
            if (id != functions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(functions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FunctionsExists(functions.Id))
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
            ViewData["BigTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", functions.BigTitleTextId);
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId", functions.LongTextId);
            ViewData["ModuleId"] = new SelectList(_context.Modules, "Id", "BigTitleTextId", functions.ModuleId);
            ViewData["SmallTitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", functions.SmallTitleTextId);
            ViewData["FullDescriptionTextId"] = new SelectList(_context.Texts, "TextId", "TextId", functions.FullDescriptionTextId);
            return View(functions);
        }

        // GET: AdminFunctions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var functions = await _context.Functions
                .Include(f => f.BigTitleText)
                .Include(f => f.LongText)
                .Include(f => f.Module)
                .Include(f => f.SmallTitleText)
                .Include(f => f.FullDescription)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (functions == null)
            {
                return NotFound();
            }

            return View(functions);
        }

        // POST: AdminFunctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var functions = await _context.Functions.FindAsync(id);
            _context.Functions.Remove(functions);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FunctionsExists(int id)
        {
            return _context.Functions.Any(e => e.Id == id);
        }
    }
}
