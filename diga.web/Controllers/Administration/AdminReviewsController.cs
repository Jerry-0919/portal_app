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
    public class AdminReviewsController : Controller
    {
        private readonly DigaContext _context;

        public AdminReviewsController(DigaContext context)
        {
            _context = context;
        }

        // GET: AdminReviews
        public async Task<IActionResult> Index()
        {
            var digaContext = _context.Reviews.Include(r => r.NameText).Include(r => r.PositionText).Include(r => r.ReviewText);
            return View(await digaContext.ToListAsync());
        }

        // GET: AdminReviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews
                .Include(r => r.NameText)
                .Include(r => r.PositionText)
                .Include(r => r.ReviewText)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reviews == null)
            {
                return NotFound();
            }

            return View(reviews);
        }

        // GET: AdminReviews/Create
        public IActionResult Create()
        {
            ViewData["NameTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["PositionTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["ReviewTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            return View();
        }

        // POST: AdminReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NameTextId,PictureUrl,ReviewTextId,PositionTextId,CompanyName")] Reviews reviews)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reviews);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NameTextId"] = new SelectList(_context.Texts, "TextId", "TextId", reviews.NameTextId);
            ViewData["PositionTextId"] = new SelectList(_context.Texts, "TextId", "TextId", reviews.PositionTextId);
            ViewData["ReviewTextId"] = new SelectList(_context.Texts, "TextId", "TextId", reviews.ReviewTextId);
            return View(reviews);
        }

        // GET: AdminReviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews.FindAsync(id);
            if (reviews == null)
            {
                return NotFound();
            }
            ViewData["NameTextId"] = new SelectList(_context.Texts, "TextId", "TextId", reviews.NameTextId);
            ViewData["PositionTextId"] = new SelectList(_context.Texts, "TextId", "TextId", reviews.PositionTextId);
            ViewData["ReviewTextId"] = new SelectList(_context.Texts, "TextId", "TextId", reviews.ReviewTextId);
            return View(reviews);
        }

        // POST: AdminReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NameTextId,PictureUrl,ReviewTextId,PositionTextId,CompanyName")] Reviews reviews)
        {
            if (id != reviews.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reviews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewsExists(reviews.Id))
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
            ViewData["NameTextId"] = new SelectList(_context.Texts, "TextId", "TextId", reviews.NameTextId);
            ViewData["PositionTextId"] = new SelectList(_context.Texts, "TextId", "TextId", reviews.PositionTextId);
            ViewData["ReviewTextId"] = new SelectList(_context.Texts, "TextId", "TextId", reviews.ReviewTextId);
            return View(reviews);
        }

        // GET: AdminReviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews
                .Include(r => r.NameText)
                .Include(r => r.PositionText)
                .Include(r => r.ReviewText)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reviews == null)
            {
                return NotFound();
            }

            return View(reviews);
        }

        // POST: AdminReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reviews = await _context.Reviews.FindAsync(id);
            _context.Reviews.Remove(reviews);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewsExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
