using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using diga.bl.Models; using diga.dal;

namespace diga.web.Controllers.Administration
{
    public class AdminAdvantagesController : Controller
    {
        private readonly DigaContext _context;

        public AdminAdvantagesController(DigaContext context)
        {
            _context = context;
        }

        // GET: AdminAdvantages
        public async Task<IActionResult> Index()
        {
            var digaContext = _context.Advantages.Include(a => a.LongText).Include(a => a.ShortText).Include(a => a.TitleText);
            return View(await digaContext.ToListAsync());
        }

        // GET: AdminAdvantages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advantages = await _context.Advantages
                .Include(a => a.LongText)
                .Include(a => a.ShortText)
                .Include(a => a.TitleText)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advantages == null)
            {
                return NotFound();
            }

            return View(advantages);
        }

        // GET: AdminAdvantages/Create
        public IActionResult Create()
        {
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["ShortTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["TitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            return View();
        }

        // POST: AdminAdvantages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PictureUrl,TitleTextId,ShortTextId,LongTextId")] Advantages advantages)
        {
            if (ModelState.IsValid)
            {
                _context.Add(advantages);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId", advantages.LongTextId);
            ViewData["ShortTextId"] = new SelectList(_context.Texts, "TextId", "TextId", advantages.ShortTextId);
            ViewData["TitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", advantages.TitleTextId);
            return View(advantages);
        }

        // GET: AdminAdvantages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advantages = await _context.Advantages.FindAsync(id);
            if (advantages == null)
            {
                return NotFound();
            }
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId", advantages.LongTextId);
            ViewData["ShortTextId"] = new SelectList(_context.Texts, "TextId", "TextId", advantages.ShortTextId);
            ViewData["TitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", advantages.TitleTextId);
            return View(advantages);
        }

        // POST: AdminAdvantages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PictureUrl,TitleTextId,ShortTextId,LongTextId")] Advantages advantages)
        {
            if (id != advantages.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(advantages);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvantagesExists(advantages.Id))
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
            ViewData["LongTextId"] = new SelectList(_context.Texts, "TextId", "TextId", advantages.LongTextId);
            ViewData["ShortTextId"] = new SelectList(_context.Texts, "TextId", "TextId", advantages.ShortTextId);
            ViewData["TitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", advantages.TitleTextId);
            return View(advantages);
        }

        // GET: AdminAdvantages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advantages = await _context.Advantages
                .Include(a => a.LongText)
                .Include(a => a.ShortText)
                .Include(a => a.TitleText)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advantages == null)
            {
                return NotFound();
            }

            return View(advantages);
        }

        // POST: AdminAdvantages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advantages = await _context.Advantages.FindAsync(id);
            _context.Advantages.Remove(advantages);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdvantagesExists(int id)
        {
            return _context.Advantages.Any(e => e.Id == id);
        }
    }
}
