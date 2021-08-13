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
    public class AdminArticlesController : Controller
    {
        private readonly DigaContext _context;

        public AdminArticlesController(DigaContext context)
        {
            _context = context;
        }

        // GET: AdminArticles
        public async Task<IActionResult> Index()
        {
            var digaContext = _context.Articles.Include(a => a.Text).Include(a => a.TitleText);
            return View(await digaContext.ToListAsync());
        }

        // GET: AdminArticles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articles = await _context.Articles
                .Include(a => a.Text)
                .Include(a => a.TitleText)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articles == null)
            {
                return NotFound();
            }

            return View(articles);
        }

        // GET: AdminArticles/Create
        public IActionResult Create()
        {
            ViewData["TextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            ViewData["TitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId");
            return View();
        }

        // POST: AdminArticles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TextId,TitleTextId,Enabled")] Articles articles)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articles);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TextId"] = new SelectList(_context.Texts, "TextId", "TextId", articles.TextId);
            ViewData["TitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", articles.TitleTextId);
            return View(articles);
        }

        // GET: AdminArticles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articles = await _context.Articles.FindAsync(id);
            if (articles == null)
            {
                return NotFound();
            }
            ViewData["TextId"] = new SelectList(_context.Texts, "TextId", "TextId", articles.TextId);
            ViewData["TitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", articles.TitleTextId);
            return View(articles);
        }

        // POST: AdminArticles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TextId,TitleTextId,Enabled")] Articles articles)
        {
            if (id != articles.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticlesExists(articles.Id))
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
            ViewData["TextId"] = new SelectList(_context.Texts, "TextId", "TextId", articles.TextId);
            ViewData["TitleTextId"] = new SelectList(_context.Texts, "TextId", "TextId", articles.TitleTextId);
            return View(articles);
        }

        // GET: AdminArticles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articles = await _context.Articles
                .Include(a => a.Text)
                .Include(a => a.TitleText)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articles == null)
            {
                return NotFound();
            }

            return View(articles);
        }

        // POST: AdminArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articles = await _context.Articles.FindAsync(id);
            _context.Articles.Remove(articles);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticlesExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
