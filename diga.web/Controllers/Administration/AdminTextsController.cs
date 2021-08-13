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
    public class AdminTextsController : Controller
    {
        private readonly DigaContext _context;

        public AdminTextsController(DigaContext context)
        {
            _context = context;
        }

        // GET: AdminTexts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Texts.ToListAsync());
        }

        // GET: AdminTexts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var texts = await _context.Texts
                .FirstOrDefaultAsync(m => m.TextId == id);
            if (texts == null)
            {
                return NotFound();
            }

            return View(texts);
        }

        // GET: AdminTexts/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateNonHtml()
        {
            return View();
        }

        // POST: AdminTexts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TextId,En,Ru,Pt,Es,IsHtml")] Texts texts)
        {
            if (ModelState.IsValid)
            {
                _context.Add(texts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(texts);
        }

        // GET: AdminTexts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var texts = await _context.Texts.FindAsync(id);
            if (texts == null)
            {
                return NotFound();
            }
            return View(texts);
        }

        public async Task<IActionResult> EditNonHtml(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var texts = await _context.Texts.FindAsync(id);
            if (texts == null)
            {
                return NotFound();
            }
            return View(texts);
        }

        // POST: AdminTexts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TextId,En,Ru,Pt,Es,IsHtml")] Texts texts)
        {
            if (id != texts.TextId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(texts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TextsExists(texts.TextId))
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
            return View(texts);
        }

        // GET: AdminTexts/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var texts = await _context.Texts
                .FirstOrDefaultAsync(m => m.TextId == id);
            if (texts == null)
            {
                return NotFound();
            }

            return View(texts);
        }

        // POST: AdminTexts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var texts = await _context.Texts.FindAsync(id);
            _context.Texts.Remove(texts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TextsExists(string id)
        {
            return _context.Texts.Any(e => e.TextId == id);
        }
    }
}
