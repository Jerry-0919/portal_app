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
    public class AdminCasesController : Controller
    {
        private readonly DigaContext _context;

        public AdminCasesController(DigaContext context)
        {
            _context = context;
        }

        // GET: AdminCases
        public async Task<IActionResult> Index()
        {
            var digaContext = _context.Cases.Include(c => c.LongText).Include(c => c.ShortText).Include(c => c.TitleText);
            return View(await digaContext.ToListAsync());
        }

        // GET: AdminCases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cases = await _context.Cases
                .Include(c => c.LongText)
                .Include(c => c.ShortText)
                .Include(c => c.TitleText)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cases == null)
            {
                return NotFound();
            }

            return View(cases);
        }

        // GET: AdminCases/Create
        public IActionResult Create()
        {
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["ShortTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["TitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            return View();
        }

        // POST: AdminCases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TitleTextId,ShortTextId,LongTextId,ReviewFileUrl,ReviewVideoUrl,CompanyName")] Cases cases)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cases);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId", cases.LongTextId);
            ViewData["ShortTextId"] = new SelectList(_context.Texts, "TextId", "TextId", cases.ShortTextId);
            ViewData["TitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", cases.TitleTextId);
            return View(cases);
        }

        // GET: AdminCases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cases = await _context.Cases.FindAsync(id);
            if (cases == null)
            {
                return NotFound();
            }
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId", cases.LongTextId);
            ViewData["ShortTextId"] = new SelectList(_context.Texts, "TextId", "TextId", cases.ShortTextId);
            ViewData["TitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", cases.TitleTextId);
            return View(cases);
        }

        // POST: AdminCases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TitleTextId,ShortTextId,LongTextId,ReviewFileUrl,ReviewVideoUrl,CompanyName")] Cases cases)
        {
            if (id != cases.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cases);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CasesExists(cases.Id))
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
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId", cases.LongTextId);
            ViewData["ShortTextId"] = new SelectList(_context.Texts, "TextId", "TextId", cases.ShortTextId);
            ViewData["TitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", cases.TitleTextId);
            return View(cases);
        }

        // GET: AdminCases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cases = await _context.Cases
                .Include(c => c.LongText)
                .Include(c => c.ShortText)
                .Include(c => c.TitleText)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cases == null)
            {
                return NotFound();
            }

            return View(cases);
        }

        // POST: AdminCases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cases = await _context.Cases.FindAsync(id);
            _context.Cases.Remove(cases);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CasesExists(int id)
        {
            return _context.Cases.Any(e => e.Id == id);
        }
    }
}
